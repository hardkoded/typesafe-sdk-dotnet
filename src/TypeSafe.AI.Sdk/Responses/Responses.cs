using System.Text.Json;
using System.Text.Json.Serialization;

namespace TypeSafe.AI.Sdk;

/// <summary>Token usage for a request.</summary>
public sealed class Usage
{
    /// <summary>Number of input tokens used.</summary>
    [JsonPropertyName("input_tokens")]
    public int InputTokens { get; init; }

    /// <summary>Number of output tokens used.</summary>
    [JsonPropertyName("output_tokens")]
    public int OutputTokens { get; init; }
}

/// <summary>Metadata for an available model.</summary>
public sealed class ModelCard
{
    /// <summary>Model identifier, for example <c>jev-1.13.0</c>.</summary>
    [JsonPropertyName("name")]
    public string Name { get; init; } = "";

    /// <summary>Human-readable description.</summary>
    [JsonPropertyName("description")]
    public string Description { get; init; } = "";

    /// <summary>Release date as returned by the API.</summary>
    [JsonPropertyName("release_date")]
    public string ReleaseDate { get; init; } = "";
}

/// <summary>Base type for a typed System One answer.</summary>
public abstract class Answer
{
    /// <summary>Answer type matching the question: <c>noul</c>, <c>choice</c>, or <c>score</c>.</summary>
    public abstract string Type { get; }
}

/// <summary>A yes/no answer. The value is the probability of yes, from 0 to 1.</summary>
public sealed class NoulAnswer : Answer
{
    /// <inheritdoc />
    public override string Type => "noul";

    /// <summary>Probability of a yes answer, from zero to one.</summary>
    public double Noul { get; init; }
}

/// <summary>A selected label and its probabilities.</summary>
public sealed class ChoiceAnswer : Answer
{
    /// <inheritdoc />
    public override string Type => "choice";

    /// <summary>The selected label.</summary>
    public string Choice { get; init; } = "";

    /// <summary>Reported confidence in the selected label.</summary>
    public double Confidence { get; init; }

    /// <summary>Probabilities keyed by label.</summary>
    public IReadOnlyDictionary<string, double> Probabilities { get; init; } =
        new Dictionary<string, double>();
}

/// <summary>An expected score with its rubric and probabilities.</summary>
public sealed class ScoreAnswer : Answer
{
    /// <inheritdoc />
    public override string Type => "score";

    /// <summary>Expected score, which may fall between integer rubric levels.</summary>
    public double Score { get; init; }

    /// <summary>Reported confidence in the score.</summary>
    public double Confidence { get; init; }

    /// <summary>Rubric descriptions keyed by score index (string keys matching the API).</summary>
    public IReadOnlyDictionary<string, JsonElement> Legend { get; init; } =
        new Dictionary<string, JsonElement>();

    /// <summary>Probabilities keyed by score index.</summary>
    public IReadOnlyDictionary<string, double> Probabilities { get; init; } =
        new Dictionary<string, double>();
}

/// <summary>Answers keyed by question name, with model and usage metadata.</summary>
public sealed class SystemOneResult
{
    /// <summary>The model used to answer the request.</summary>
    public string Model { get; init; } = "";

    /// <summary>Answers keyed by the same names supplied in the request.</summary>
    public IReadOnlyDictionary<string, Answer> Answers { get; init; } =
        new Dictionary<string, Answer>();

    /// <summary>Token usage for the request.</summary>
    public Usage Usage { get; init; } = new();

    /// <summary>Return the named answer as <typeparamref name="T"/>.</summary>
    public T Get<T>(string name) where T : Answer
    {
        if (!Answers.TryGetValue(name, out var answer))
        {
            throw new TypeSafeException($"No answer named \"{name}\".");
        }

        if (answer is not T typed)
        {
            throw new TypeSafeException(
                $"Answer \"{name}\" is {answer.Type}, not {typeof(T).Name}.");
        }

        return typed;
    }

    /// <summary>Return the named Noul answer.</summary>
    public NoulAnswer GetNoul(string name) => Get<NoulAnswer>(name);

    /// <summary>Return the named Choice answer.</summary>
    public ChoiceAnswer GetChoice(string name) => Get<ChoiceAnswer>(name);

    /// <summary>Return the named Score answer.</summary>
    public ScoreAnswer GetScore(string name) => Get<ScoreAnswer>(name);
}

/// <summary>Parsed data with its HTTP response and request ID. Mirrors JS <c>WithResponse</c>.</summary>
public sealed class TypeSafeResponse<T>
{
    /// <summary>The parsed response body.</summary>
    public T Data { get; }

    /// <summary>The HTTP response, with its body already consumed by parsing.</summary>
    public HttpResponseMessage Response { get; }

    /// <summary>Request ID from <c>x-typesafe-request-id</c>, or <c>null</c> when absent.</summary>
    public string? RequestId { get; }

    /// <summary>Create a typed response wrapper.</summary>
    public TypeSafeResponse(T data, HttpResponseMessage response, string? requestId)
    {
        Data = data;
        Response = response;
        RequestId = requestId;
    }
}
