// Copyright (c) Dario Kondratiuk.
// Licensed under the MIT License.

using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace TypeSafe.AI.Sdk;

/// <summary>An image sent as its own part of a <c>multipart/form-data</c> body.</summary>
internal sealed class AssetPart
{
    public AssetPart(string alias, Image image, string name)
    {
        Alias = alias;
        Image = image;
        Name = name;
    }

    /// <summary>The name or reference the payload uses for the image.</summary>
    public string Alias { get; }

    public Image Image { get; }

    /// <summary>The name of the body part that holds the image file bytes.</summary>
    public string Name { get; }

    /// <summary>The content type identified by the file signature, or <c>application/octet-stream</c>.</summary>
    public string ContentType =>
        StartsWith(Image.Bytes, s_pngSignature) ? "image/png"
        : StartsWith(Image.Bytes, s_jpegSignature) ? "image/jpeg"
        : "application/octet-stream";

    /// <summary>The PNG file signature (PNG spec 5.2).</summary>
    private static readonly byte[] s_pngSignature = { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };

    /// <summary>The JPEG start-of-image marker (ITU T.81).</summary>
    private static readonly byte[] s_jpegSignature = { 0xFF, 0xD8, 0xFF };

    private static bool StartsWith(byte[] bytes, byte[] signature) =>
        bytes.Length >= signature.Length && bytes.AsSpan(0, signature.Length).SequenceEqual(signature);
}

/// <summary>
/// Writes images into a request payload, as their names in <see cref="SystemOneRequest.Assets"/> or else
/// their references, and collects the images the request sends.
/// </summary>
internal sealed class AssetContext
{
    /// <summary>Matches <see cref="Image.ToString"/>: the reference an image is sent under.</summary>
    public static readonly Regex Reference = new("<image:[0-9a-f]{32}>", RegexOptions.CultureInvariant);

    private static readonly JsonSerializerOptions s_relaxed = new() { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };

    private readonly IDictionary<string, Image> _named;
    private readonly Dictionary<Image, string> _names = new();
    private readonly Dictionary<string, Image> _known = new(StringComparer.Ordinal);

    public AssetContext(IDictionary<string, Image>? named)
    {
        _named = named ?? new Dictionary<string, Image>();
        foreach (var pair in _named)
        {
            if (pair.Value is null)
            {
                throw new TypeSafeException($"Asset \"{pair.Key}\" is null.");
            }

            // An image under several names is written as the last one.
            _names[pair.Value] = pair.Key;
        }
    }

    public JsonNode WriteImage(Image image)
    {
        if (_names.TryGetValue(image, out var name))
        {
            return JsonValue.Create(name)!;
        }

        _known[image.ToString()] = image;
        return JsonValue.Create(image.ToString())!;
    }

    public JsonNode WriteText(TextWithAssets text)
    {
        var result = text.Text;
        foreach (var asset in text.Assets)
        {
            if (_names.TryGetValue(asset, out var name))
            {
                result = result.Replace(asset.ToString(), name);
            }
            else
            {
                _known[asset.ToString()] = asset;
            }
        }

        return JsonValue.Create(result)!;
    }

    /// <summary>
    /// The images the request sends: those in <see cref="SystemOneRequest.Assets"/>, then each image whose
    /// reference appears in <paramref name="payload"/>, in order of appearance.
    /// </summary>
    /// <exception cref="TypeSafeException">
    /// The payload holds the reference of an image not attached to the request, or doesn't contain the name of
    /// an image in <see cref="SystemOneRequest.Assets"/>.
    /// </exception>
    public IReadOnlyList<AssetPart> Resolve(JsonObject payload)
    {
        // Names are matched as they appear in the JSON, with quotes and backslashes escaped.
        var json = payload.ToJsonString(s_relaxed);
        var unreferenced = _named.Keys
            .Where(name => !json.Contains(Unquoted(JsonSerializer.Serialize(name, s_relaxed))))
            .ToList();
        if (unreferenced.Count > 0)
        {
            throw new TypeSafeException(
                $"Assets not referenced in the request: {string.Join(", ", unreferenced)}. " +
                "Write each name in `Assets` into `State`, question instructions, or criteria.");
        }

        var sent = new List<(string Alias, Image Image)>();
        var aliases = new HashSet<string>(StringComparer.Ordinal);
        foreach (var pair in _named)
        {
            sent.Add((pair.Key, pair.Value));
            aliases.Add(pair.Key);
        }

        var missing = new List<string>();
        foreach (Match match in Reference.Matches(json))
        {
            if (aliases.Contains(match.Value))
            {
                continue;
            }

            if (_known.TryGetValue(match.Value, out var image))
            {
                sent.Add((match.Value, image));
                aliases.Add(match.Value);
            }
            else if (!missing.Contains(match.Value))
            {
                missing.Add(match.Value);
            }
        }

        if (missing.Count > 0)
        {
            throw new TypeSafeException(
                $"Image references without an attached image: {string.Join(", ", missing)}. " +
                "Interpolate images with `TextWithAssets.From`.");
        }

        // The API accepts each part for only one asset, so an image under several references is sent once per reference.
        return sent.Select((entry, index) => new AssetPart(entry.Alias, entry.Image, $"asset-{index}")).ToList();
    }

    private static string Unquoted(string json) => json.Substring(1, json.Length - 2);
}
