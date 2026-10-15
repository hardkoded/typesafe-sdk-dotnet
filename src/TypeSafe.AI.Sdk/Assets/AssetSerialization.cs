// Copyright (c) Dario Kondratiuk.
// Licensed under the MIT License.

using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace TypeSafe.AI.Sdk;

/// <summary>
/// Serialize a System One request, writing each asset as its name in <c>assets</c>, or else as
/// its reference, and collect the assets the request sends. Mirrors JS <c>serializeRequest</c>
/// / <c>encodeAssets</c>.
/// </summary>
internal static class AssetSerialization
{
    public static PreparedHttpBody PrepareSystemOne(SystemOneRequest request, string defaultModel)
    {
        var named = request.Assets is null
            ? new Dictionary<string, Image>()
            : new Dictionary<string, Image>(request.Assets);
        var context = new AssetWriteContext(named);
        var payload = AnswerParser.BuildSystemOnePayload(request, defaultModel, context);
        var json = payload.ToJsonString(JsonValues.SerializerOptions);
        var sent = context.CollectSentAssets(json);
        if (sent.Count == 0)
        {
            return new PreparedHttpBody(json, assetParts: null);
        }

        var parts = new List<PreparedAssetPart>(sent.Count);
        var assetsNode = new JsonObject();
        foreach (var pair in sent)
        {
            var name = "asset-" + parts.Count.ToString(System.Globalization.CultureInfo.InvariantCulture);
            var bytes = pair.Value.CopyBytes();
            parts.Add(new PreparedAssetPart(name, Image.ContentTypeOf(bytes), bytes));
            assetsNode[pair.Key] = new JsonObject
            {
                ["type"] = "image",
                ["part"] = name,
            };
        }

        payload["assets"] = assetsNode;
        return new PreparedHttpBody(payload.ToJsonString(JsonValues.SerializerOptions), parts);
    }
}

/// <summary>JSON payload plus optional multipart image parts, ready to send (and retry).</summary>
internal sealed class PreparedHttpBody
{
    public string Json { get; }

    public IReadOnlyList<PreparedAssetPart>? AssetParts { get; }

    public bool HasAssets => AssetParts is { Count: > 0 };

    public PreparedHttpBody(string json, IReadOnlyList<PreparedAssetPart>? assetParts)
    {
        Json = json;
        AssetParts = assetParts;
    }

    public HttpContent CreateContent()
    {
        if (!HasAssets)
        {
            return new StringContent(Json, Encoding.UTF8, "application/json");
        }

        var multipart = new MultipartFormDataContent();
        multipart.Add(new StringContent(Json, Encoding.UTF8, "application/json"), "request");
        foreach (var part in AssetParts!)
        {
            var content = new ByteArrayContent(part.Bytes);
            content.Headers.ContentType = new MediaTypeHeaderValue(part.ContentType);
            multipart.Add(content, part.Name, part.FileName);
        }

        return multipart;
    }
}

internal sealed class PreparedAssetPart
{
    public string Name { get; }

    public string FileName => Name;

    public string ContentType { get; }

    public byte[] Bytes { get; }

    public PreparedAssetPart(string name, string contentType, byte[] bytes)
    {
        Name = name;
        ContentType = contentType;
        Bytes = bytes;
    }
}

/// <summary>
/// Tracks named assets and images encountered while writing JSON, then validates references.
/// </summary>
internal sealed class AssetWriteContext
{
    private readonly Dictionary<string, Image> _named;
    private readonly Dictionary<Image, string> _names;
    private readonly Dictionary<string, Image> _known;
    private readonly JsonSerializerOptions _serializerOptions;

    public JsonSerializerOptions SerializerOptions => _serializerOptions;

    public AssetWriteContext(Dictionary<string, Image> named)
    {
        _named = named;
        _names = new Dictionary<Image, string>();
        foreach (var pair in named)
        {
            // An asset under several names is written as the last one.
            _names[pair.Value] = pair.Key;
        }

        _known = new Dictionary<string, Image>(StringComparer.Ordinal);
        foreach (var asset in named.Values)
        {
            _known[asset.ToString()] = asset;
        }

        _serializerOptions = new JsonSerializerOptions(JsonValues.SerializerOptions);
        _serializerOptions.Converters.Add(new ImageJsonConverter(this));
        _serializerOptions.Converters.Add(new TextWithAssetsJsonConverter(this));
    }

    public string WriteImage(Image image)
    {
        if (_names.TryGetValue(image, out var name))
        {
            return name;
        }

        _known[image.ToString()] = image;
        return image.ToString();
    }

    public string WriteText(TextWithAssets original)
    {
        var text = original.Text;
        foreach (var asset in original.Assets)
        {
            if (_names.TryGetValue(asset, out var name))
            {
                text = text.Replace(asset.ToString(), name);
            }
            else
            {
                _known[asset.ToString()] = asset;
            }
        }

        return text;
    }

    /// <summary>
    /// The assets the request sends: those in <c>assets</c>, then each asset whose reference
    /// appears in the JSON, in order of appearance.
    /// </summary>
    /// <exception cref="TypeSafeException">
    /// The JSON holds the reference of an asset not attached to the request, or doesn't contain
    /// the name of an asset in <c>assets</c>.
    /// </exception>
    public Dictionary<string, Image> CollectSentAssets(string json)
    {
        var unreferenced = new List<string>();
        foreach (var name in _named.Keys)
        {
            if (json.IndexOf(JsonStringContents(name), StringComparison.Ordinal) < 0)
            {
                unreferenced.Add(name);
            }
        }

        if (unreferenced.Count > 0)
        {
            throw new TypeSafeException(
                "Assets not referenced in the request: " + string.Join(", ", unreferenced) + ". " +
                "Write each name in `assets` into `state`, question instructions, or criteria.");
        }

        var sent = new Dictionary<string, Image>(StringComparer.Ordinal);
        foreach (var pair in _named)
        {
            sent[pair.Key] = pair.Value;
        }

        var missing = new List<string>();
        var missingSet = new HashSet<string>(StringComparer.Ordinal);
        foreach (Match match in Image.s_referencePattern.Matches(json))
        {
            var reference = match.Value;
            if (sent.ContainsKey(reference))
            {
                continue;
            }

            if (_known.TryGetValue(reference, out var asset))
            {
                sent[reference] = asset;
            }
            else if (missingSet.Add(reference))
            {
                missing.Add(reference);
            }
        }

        if (missing.Count > 0)
        {
            throw new TypeSafeException(
                "Image references without an attached image: " + string.Join(", ", missing) + ". " +
                "Tag template literals that interpolate images with `withAssets`.");
        }

        return sent;
    }

    /// <summary>
    /// Names are matched as they appear in the JSON, with quotes and backslashes escaped
    /// (JS <c>JSON.stringify(name).slice(1, -1)</c>).
    /// </summary>
    private static string JsonStringContents(string value)
    {
        var json = JsonSerializer.Serialize(value);
#if NET
        return json[1..^1];
#else
        return json.Substring(1, json.Length - 2);
#endif
    }
}
