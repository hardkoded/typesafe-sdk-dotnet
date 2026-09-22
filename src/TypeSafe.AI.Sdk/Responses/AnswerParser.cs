// Copyright (c) Hardkoded.
// Licensed under the MIT License.

using System.Text.Json;
using System.Text.Json.Nodes;

namespace TypeSafe.AI.Sdk;

internal static class AnswerParser
{
    public static SystemOneResult ParseSystemOne(object? body)
    {
        var element = AsObject(body, "POST /v1/systemone");
        var answers = new Dictionary<string, Answer>(StringComparer.Ordinal);
        if (element.TryGetProperty("answers", out var answersNode) && answersNode.ValueKind == JsonValueKind.Object)
        {
            foreach (var property in answersNode.EnumerateObject())
            {
                answers[property.Name] = ParseAnswer(property.Value, property.Name);
            }
        }

        var usage = new Usage();
        if (element.TryGetProperty("usage", out var usageNode) && usageNode.ValueKind == JsonValueKind.Object)
        {
            usage = new Usage
            {
                InputTokens = ReadInt(usageNode, "input_tokens"),
                OutputTokens = ReadInt(usageNode, "output_tokens"),
            };
        }

        return new SystemOneResult
        {
            Model = element.TryGetProperty("model", out var model) && model.ValueKind == JsonValueKind.String
                ? model.GetString() ?? ""
                : "",
            Answers = answers,
            Usage = usage,
        };
    }

    public static IReadOnlyList<ModelCard> ParseModels(object? body)
    {
        if (body is not JsonElement element || element.ValueKind != JsonValueKind.Object
            || !element.TryGetProperty("models", out var models) || models.ValueKind != JsonValueKind.Array)
        {
            throw new TypeSafeException(
                "Unexpected response shape from GET /v1/models; expected { models: [...] }.");
        }

        var cards = new List<ModelCard>();
        foreach (var item in models.EnumerateArray())
        {
            cards.Add(new ModelCard
            {
                Name = item.TryGetProperty("name", out var name) ? name.GetString() ?? "" : "",
                Description = item.TryGetProperty("description", out var description) ? description.GetString() ?? "" : "",
                ReleaseDate = item.TryGetProperty("release_date", out var release) ? release.GetString() ?? "" : "",
            });
        }

        return cards;
    }

    private static Answer ParseAnswer(JsonElement node, string name)
    {
        var type = node.TryGetProperty("type", out var typeNode) && typeNode.ValueKind == JsonValueKind.String
            ? typeNode.GetString()
            : null;

        return type switch
        {
            "noul" => new NoulAnswer
            {
                Noul = node.TryGetProperty("noul", out var noul) ? noul.GetDouble() : 0,
            },
            "choice" => new ChoiceAnswer
            {
                Choice = node.TryGetProperty("choice", out var choice) ? choice.GetString() ?? "" : "",
                Confidence = node.TryGetProperty("confidence", out var confidence) ? confidence.GetDouble() : 0,
                Probabilities = ReadDoubleMap(node, "probabilities"),
            },
            "score" => new ScoreAnswer
            {
                Score = node.TryGetProperty("score", out var score) ? score.GetDouble() : 0,
                Confidence = node.TryGetProperty("confidence", out var confidence) ? confidence.GetDouble() : 0,
                Legend = ReadElementMap(node, "legend"),
                Probabilities = ReadDoubleMap(node, "probabilities"),
            },
            _ => throw new TypeSafeException($"Unexpected answer type \"{type}\" for \"{name}\"."),
        };
    }

    private static IReadOnlyDictionary<string, double> ReadDoubleMap(JsonElement node, string name)
    {
        var map = new Dictionary<string, double>(StringComparer.Ordinal);
        if (!node.TryGetProperty(name, out var property) || property.ValueKind != JsonValueKind.Object)
        {
            return map;
        }

        foreach (var item in property.EnumerateObject())
        {
            if (item.Value.ValueKind is JsonValueKind.Number)
            {
                map[item.Name] = item.Value.GetDouble();
            }
        }

        return map;
    }

    private static IReadOnlyDictionary<string, JsonElement> ReadElementMap(JsonElement node, string name)
    {
        var map = new Dictionary<string, JsonElement>(StringComparer.Ordinal);
        if (!node.TryGetProperty(name, out var property) || property.ValueKind != JsonValueKind.Object)
        {
            return map;
        }

        foreach (var item in property.EnumerateObject())
        {
            map[item.Name] = item.Value.Clone();
        }

        return map;
    }

    private static int ReadInt(JsonElement node, string name) =>
        node.TryGetProperty(name, out var property) && property.ValueKind == JsonValueKind.Number
            ? property.GetInt32()
            : 0;

    private static JsonElement AsObject(object? body, string endpoint)
    {
        if (body is JsonElement element && element.ValueKind == JsonValueKind.Object)
        {
            return element;
        }

        throw new TypeSafeException($"Unexpected response shape from {endpoint}; expected a JSON object.");
    }

    public static JsonObject BuildSystemOnePayload(SystemOneRequest request, string defaultModel)
    {
        Question.Validate(new Dictionary<string, Question>(request.Questions));
        var payload = new JsonObject
        {
            ["state"] = JsonValues.ToNode(request.State),
            ["model"] = request.Model ?? defaultModel,
            ["questions"] = BuildQuestions(request.Questions),
        };

        if (request.Extra is not null)
        {
            foreach (var pair in request.Extra)
            {
                payload[pair.Key] = JsonValues.ToNode(pair.Value);
            }
        }

        return payload;
    }

    private static JsonObject BuildQuestions(IDictionary<string, Question> questions)
    {
        var node = new JsonObject();
        foreach (var pair in questions)
        {
            node[pair.Key] = QuestionWire.ToJson(pair.Value);
        }

        return node;
    }
}
