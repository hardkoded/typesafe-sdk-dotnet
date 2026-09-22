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

    public static JsonNode? ToNode(object? value)
    {
        switch (value)
        {
            case null:
                return null;
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
                    obj[pair.Key] = ToNode(pair.Value);
                }

                return obj;
            }
            case IDictionary map:
            {
                var obj = new JsonObject();
                foreach (DictionaryEntry entry in map)
                {
                    obj[Convert.ToString(entry.Key, System.Globalization.CultureInfo.InvariantCulture) ?? ""] =
                        ToNode(entry.Value);
                }

                return obj;
            }
            case IEnumerable enumerable when value is not string:
            {
                var array = new JsonArray();
                foreach (var item in enumerable)
                {
                    array.Add(ToNode(item));
                }

                return array;
            }
            default:
                return JsonSerializer.SerializeToNode(value, value.GetType(), SerializerOptions);
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
