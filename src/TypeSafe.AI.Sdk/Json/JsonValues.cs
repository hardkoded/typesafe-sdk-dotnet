// Copyright (c) Dario Kondratiuk.
// Licensed under the MIT License.

using System.Collections;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace TypeSafe.AI.Sdk;

internal static class JsonValues
{
    public static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = false,
    };

    public static JsonNode? ToNode(object? value) => ToNode(value, context: null);

    internal static JsonNode? ToNode(object? value, AssetWriteContext? context)
    {
        switch (value)
        {
            case null:
                return null;
            case Image image:
                return JsonValue.Create(context is null ? image.ToString() : context.WriteImage(image));
            case TextWithAssets text:
                return JsonValue.Create(context is null ? text.Text : context.WriteText(text));
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
                return JsonSerializer.SerializeToNode(value, Options(context));
            case IDictionary<string, object?> map:
            {
                var obj = new JsonObject();
                foreach (var pair in map)
                {
                    obj[pair.Key] = ToNode(pair.Value, context);
                }

                return obj;
            }
            case IDictionary map:
            {
                var obj = new JsonObject();
                foreach (DictionaryEntry entry in map)
                {
                    obj[Convert.ToString(entry.Key, System.Globalization.CultureInfo.InvariantCulture) ?? ""] =
                        ToNode(entry.Value, context);
                }

                return obj;
            }
            case IEnumerable enumerable when value is not string:
            {
                var array = new JsonArray();
                foreach (var item in enumerable)
                {
                    array.Add(ToNode(item, context));
                }

                return array;
            }
            default:
                return JsonSerializer.SerializeToNode(value, value.GetType(), Options(context));
        }
    }

    private static JsonSerializerOptions Options(AssetWriteContext? context) =>
        context?.SerializerOptions ?? SerializerOptions;

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
