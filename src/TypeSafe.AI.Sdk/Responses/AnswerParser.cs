// Copyright (c) Dario Kondratiuk.
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
        if (
            !element.TryGetProperty("answers", out var answersNode)
            || answersNode.ValueKind != JsonValueKind.Object
        )
        {
            throw new TypeSafeException(
                "Unexpected response shape from POST /v1/systemone; expected `answers` to be an object."
            );
        }

        foreach (var property in answersNode.EnumerateObject())
        {
            answers[property.Name] = ParseAnswer(property.Value, property.Name);
        }

        var usage = new Usage();
        if (
            element.TryGetProperty("usage", out var usageNode)
            && usageNode.ValueKind == JsonValueKind.Object
        )
        {
            usage = new Usage
            {
                InputTokens = ReadNullableInt(usageNode, "input_tokens"),
                OutputTokens = ReadNullableInt(usageNode, "output_tokens"),
            };
        }

        return new SystemOneResult
        {
            Model =
                element.TryGetProperty("model", out var model)
                && model.ValueKind == JsonValueKind.String
                    ? model.GetString() ?? ""
                    : "",
            Answers = answers,
            Usage = usage,
        };
    }

    public static IReadOnlyList<ModelCard> ParseModels(object? body)
    {
        if (
            body is not JsonElement element
            || element.ValueKind != JsonValueKind.Object
            || !element.TryGetProperty("models", out var models)
            || models.ValueKind != JsonValueKind.Array
        )
        {
            throw new TypeSafeException(
                "Unexpected response shape from GET /v1/models; expected { models: [...] }."
            );
        }

        List<ModelCard> cards = [];
        foreach (var item in models.EnumerateArray())
        {
            cards.Add(
                new ModelCard
                {
                    Name = item.TryGetProperty("name", out var name) ? name.GetString() ?? "" : "",
                    Description = item.TryGetProperty("description", out var description)
                        ? description.GetString() ?? ""
                        : "",
                    ReleaseDate = item.TryGetProperty("release_date", out var release)
                        ? release.GetString() ?? ""
                        : "",
                }
            );
        }

        return cards;
    }

    private static Answer ParseAnswer(JsonElement node, string name)
    {
        var type =
            node.TryGetProperty("type", out var typeNode)
            && typeNode.ValueKind == JsonValueKind.String
                ? typeNode.GetString()
                : null;

        return type switch
        {
            "noul" => new NoulAnswer { Noul = RequireNumber(node, "noul", "noul", name) },
            "choice" => ParseChoice(node, name),
            "score" => ParseScore(node, name),
            "box" => new BoxAnswer
            {
                Box = ReadBox(node, name),
                Confidence = ReadOptionalNumber(node, "confidence"),
            },
            _ => throw new TypeSafeException($"Unexpected answer type \"{type}\" for \"{name}\"."),
        };
    }

    private static ChoiceAnswer ParseChoice(JsonElement node, string name)
    {
        if (
            !node.TryGetProperty("choice", out var choice)
            || choice.ValueKind != JsonValueKind.String
        )
        {
            throw Malformed("choice", "choice", name, "a string");
        }

        var reported = ReadOptionalNumber(node, "confidence");
        return new ChoiceAnswer
        {
            Choice = choice.GetString() ?? "",
            Confidence = reported ?? 0,
            ReportedConfidence = reported,
            Probabilities = RequireDoubleMap(node, "choice", name),
        };
    }

    private static ScoreAnswer ParseScore(JsonElement node, string name)
    {
        var score = RequireNumber(node, "score", "score", name);
        var reported = ReadOptionalNumber(node, "confidence");
        return new ScoreAnswer
        {
            Score = score,
            Confidence = reported ?? 0,
            ReportedConfidence = reported,
            Legend = ReadElementMap(node, "legend"),
            Probabilities = RequireDoubleMap(node, "score", name),
        };
    }

    private static TypeSafeException Malformed(
        string type,
        string field,
        string name,
        string expected
    ) =>
        new(
            $"Malformed {type} answer for \"{name}\": field \"{field}\" is missing or is not {expected}."
        );

    private static double RequireNumber(JsonElement node, string field, string type, string name) =>
        node.TryGetProperty(field, out var property) && property.ValueKind == JsonValueKind.Number
            ? property.GetDouble()
            : throw Malformed(type, field, name, "a number");

    private static double? ReadOptionalNumber(JsonElement node, string field) =>
        node.TryGetProperty(field, out var property) && property.ValueKind == JsonValueKind.Number
            ? property.GetDouble()
            : null;

    private static BoxCoordinates ReadBox(JsonElement node, string name)
    {
        if (!node.TryGetProperty("box", out var box) || box.ValueKind != JsonValueKind.Object)
        {
            throw Malformed("box", "box", name, "an object");
        }

        return new BoxCoordinates
        {
            Top = RequireBoxEdge(box, "top", name),
            Bottom = RequireBoxEdge(box, "bottom", name),
            Left = RequireBoxEdge(box, "left", name),
            Right = RequireBoxEdge(box, "right", name),
        };
    }

    private static double RequireBoxEdge(JsonElement box, string edge, string name) =>
        box.TryGetProperty(edge, out var property) && property.ValueKind == JsonValueKind.Number
            ? property.GetDouble()
            : throw Malformed("box", $"box.{edge}", name, "a number");

    private static IReadOnlyDictionary<string, double> RequireDoubleMap(
        JsonElement node,
        string type,
        string name
    )
    {
        if (
            !node.TryGetProperty("probabilities", out var property)
            || property.ValueKind != JsonValueKind.Object
        )
        {
            throw Malformed(type, "probabilities", name, "an object of numbers");
        }

        var map = new Dictionary<string, double>(StringComparer.Ordinal);
        foreach (var item in property.EnumerateObject())
        {
            if (item.Value.ValueKind != JsonValueKind.Number)
            {
                throw Malformed(type, $"probabilities.{item.Name}", name, "a number");
            }

            map[item.Name] = item.Value.GetDouble();
        }

        return map;
    }

    private static IReadOnlyDictionary<string, JsonElement> ReadElementMap(
        JsonElement node,
        string name
    )
    {
        var map = new Dictionary<string, JsonElement>(StringComparer.Ordinal);
        if (
            !node.TryGetProperty(name, out var property)
            || property.ValueKind != JsonValueKind.Object
        )
        {
            return map;
        }

        foreach (var item in property.EnumerateObject())
        {
            map[item.Name] = item.Value.Clone();
        }

        return map;
    }

    private static int? ReadNullableInt(JsonElement node, string name) =>
        node.TryGetProperty(name, out var property)
        && property.ValueKind == JsonValueKind.Number
        && property.TryGetInt32(out var value)
            ? value
            : null;

    private static JsonElement AsObject(object? body, string endpoint)
    {
        if (body is JsonElement element && element.ValueKind == JsonValueKind.Object)
        {
            return element;
        }

        throw new TypeSafeException(
            $"Unexpected response shape from {endpoint}; expected a JSON object."
        );
    }

    public static (JsonObject Payload, IReadOnlyList<AssetPart> Assets) BuildSystemOnePayload(
        SystemOneRequest request,
        string defaultModel
    )
    {
        Question.Validate(new Dictionary<string, Question>(request.Questions));
        var assets = new AssetContext(request.Assets);
        var payload = new JsonObject
        {
            ["state"] = JsonValues.ToNode(request.State, assets),
            ["model"] = request.Model ?? defaultModel,
            ["questions"] = BuildQuestions(request.Questions, assets),
        };

        if (request.Extra is not null)
        {
            foreach (var pair in request.Extra)
            {
                payload[pair.Key] = JsonValues.ToNode(pair.Value, assets);
            }
        }

        var parts = assets.Resolve(payload);
        if (parts.Count > 0)
        {
            if (payload.ContainsKey("assets"))
            {
                throw new TypeSafeException(
                    "`assets` is reserved for images; remove it from `Extra`."
                );
            }

            var entries = new JsonObject();
            foreach (var part in parts)
            {
                entries[part.Alias] = new JsonObject { ["type"] = "image", ["part"] = part.Name };
            }

            payload["assets"] = entries;
        }

        return (payload, parts);
    }

    private static JsonObject BuildQuestions(
        IDictionary<string, Question> questions,
        AssetContext assets
    )
    {
        var node = new JsonObject();
        foreach (var pair in questions)
        {
            node[pair.Key] = QuestionWire.ToJson(pair.Value, assets);
        }

        return node;
    }
}
