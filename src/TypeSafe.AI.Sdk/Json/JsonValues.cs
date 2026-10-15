// Copyright (c) Dario Kondratiuk.
// Licensed under the MIT License.

using System.Collections;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace TypeSafe.AI.Sdk;

internal static class JsonValues
{
    public static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = false,
        Converters = { new ImageConverter(), new TextWithAssetsConverter() },
    };

    /// <summary>The assets of the request being serialized on this thread, for images inside POCOs and anonymous types.</summary>
    [ThreadStatic]
    private static AssetContext? s_assets;

    internal static AssetContext WriteAssets() => s_assets ?? new AssetContext(null);

    private static JsonNode? SerializeWithAssets(object value, AssetContext? assets)
    {
        var previous = s_assets;
        s_assets = assets;
        try
        {
            return JsonSerializer.SerializeToNode(value, value.GetType(), SerializerOptions);
        }
        finally
        {
            s_assets = previous;
        }
    }

    public static JsonNode? ToNode(object? value, AssetContext? assets = null)
    {
        switch (value)
        {
            case null:
                return null;
            case Image image:
                return (assets ?? new AssetContext(null)).WriteImage(image);
            case TextWithAssets text:
                return (assets ?? new AssetContext(null)).WriteText(text);
            case JsonNode node:
                return node.DeepClone();
            case JsonElement element:
                return JsonNode.Parse(element.GetRawText());
            case JsonDocument document:
                return JsonNode.Parse(document.RootElement.GetRawText());
            case string text:
                return JsonValue.Create(text);
            case bool flag:
                return JsonValue.Create(flag);
            case byte or sbyte or short or ushort or int or uint or long or ulong:
                return JsonValue.Create(Convert.ToInt64(value, System.Globalization.CultureInfo.InvariantCulture));
            case float or double or decimal:
                return JsonSerializer.SerializeToNode(value, SerializerOptions);
            case IDictionary<string, object?> map:
                {
                    var obj = new JsonObject();
                    foreach (var pair in map)
                    {
                        obj[pair.Key] = ToNode(pair.Value, assets);
                    }

                    return obj;
                }
            case IDictionary map:
                {
                    var obj = new JsonObject();
                    foreach (DictionaryEntry entry in map)
                    {
                        obj[Convert.ToString(entry.Key, System.Globalization.CultureInfo.InvariantCulture) ?? ""] =
                            ToNode(entry.Value, assets);
                    }

                    return obj;
                }
            case IEnumerable enumerable when value is not string:
                {
                    var array = new JsonArray();
                    foreach (var item in enumerable)
                    {
                        array.Add(ToNode(item, assets));
                    }

                    return array;
                }
            default:
                return SerializeWithAssets(value, assets);
        }
    }

    public static object? ParseBody(string text, string? contentType)
    {
        if (text.Length == 0)
        {
            return null;
        }

        var looksJson = contentType?.IndexOf("application/json", StringComparison.OrdinalIgnoreCase) >= 0;
        if (looksJson)
        {
            try
            {
                return JsonSerializer.Deserialize<JsonElement>(text, SerializerOptions);
            }
            catch (JsonException)
            {
                return text;
            }
        }

        try
        {
            return JsonSerializer.Deserialize<JsonElement>(text, SerializerOptions);
        }
        catch (JsonException)
        {
            return text;
        }
    }
}

internal sealed class ImageConverter : JsonConverter<Image>
{
    public override Image Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        throw new NotSupportedException("Images are write-only.");

    public override void Write(Utf8JsonWriter writer, Image value, JsonSerializerOptions options) =>
        JsonValues.WriteAssets().WriteImage(value).WriteTo(writer);
}

internal sealed class TextWithAssetsConverter : JsonConverter<TextWithAssets>
{
    public override TextWithAssets Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        throw new NotSupportedException("Text with assets is write-only.");

    public override void Write(Utf8JsonWriter writer, TextWithAssets value, JsonSerializerOptions options) =>
        JsonValues.WriteAssets().WriteText(value).WriteTo(writer);
}
