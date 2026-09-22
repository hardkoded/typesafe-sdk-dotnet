// Copyright (c) Dario Kondratiuk.
// Licensed under the MIT License.

using System.Collections;
using System.Text.Json.Nodes;

namespace TypeSafe.AI.Sdk;

/// <summary>A TypeSafe question identified by its <c>type</c> field.</summary>
public abstract class Question
{
    /// <summary>Question type: <c>noul</c>, <c>choice</c>, or <c>score</c>.</summary>
    public abstract string Type { get; }

    /// <summary>
    /// The question as text, a JSON object, or an array. Omitted when the builder was not given
    /// instructions (except <see cref="Noul(object?, NoulCriteria?)"/>, which defaults to <c>null</c>).
    /// </summary>
    public object? Instructions { get; }

    internal bool IncludeInstructions { get; }

    private protected Question(object? instructions, bool includeInstructions)
    {
        Instructions = instructions;
        IncludeInstructions = includeInstructions;
    }

    /// <summary>
    /// Create a yes/no question with optional descriptions for either outcome.
    /// Mirrors JS <c>noul()</c>.
    /// </summary>
    /// <param name="instructions">The question as text, a JSON object or array; defaults to <c>null</c>.</param>
    /// <param name="criteria">Optional descriptions of the yes and no outcomes.</param>
    public static NoulQuestion Noul(object? instructions = null, NoulCriteria? criteria = null) =>
        new(instructions, criteria, includeInstructions: true, includeCriteria: criteria is not null);

    /// <summary>
    /// Create a yes/no question, including an explicit <c>null</c> criteria value on the wire.
    /// </summary>
    public static NoulQuestion Noul(object? instructions, NoulCriteria? criteria, bool includeCriteria) =>
        new(instructions, criteria, includeInstructions: true, includeCriteria: includeCriteria);

    /// <summary>
    /// Create a question that selects between named alternatives.
    /// Mirrors JS <c>choice()</c>.
    /// </summary>
    /// <param name="instructions">The question as text, a JSON object or array, or <c>null</c>.</param>
    /// <param name="criteria">Labels mapped to descriptions, or <c>null</c> for undescribed labels.</param>
    public static ChoiceQuestion Choice(object? instructions, IReadOnlyDictionary<string, object?> criteria)
    {
        if (criteria is IList)
        {
            throw new TypeSafeException("Choice criteria must be a map of labels to descriptions, not a list.");
        }

        return new ChoiceQuestion(instructions, criteria, includeInstructions: true);
    }

    /// <inheritdoc cref="Choice(object?, IReadOnlyDictionary{string, object?})" />
    public static ChoiceQuestion Choice(object? instructions, Dictionary<string, object?> criteria) =>
        Choice(instructions, (IReadOnlyDictionary<string, object?>)criteria);

    /// <summary>
    /// Create a question that selects between named alternatives, from label/description pairs.
    /// Duplicate labels throw. Pass a dictionary when you already have a map.
    /// </summary>
    public static ChoiceQuestion Choice(
        object? instructions,
        params (string Label, object? Description)[] criteria) =>
        Choice(instructions, NamedMap(criteria, "choice label"));

    /// <summary>
    /// Build a name → question map from labeled pairs. Duplicate names throw.
    /// Use this when you need a dictionary (for example <see cref="SystemOneRequest.Questions"/>,
    /// or a <c>model</c> / cancellation token on the dictionary <c>SystemOneAsync</c> overload).
    /// </summary>
    public static Dictionary<string, Question> Map(params (string Name, Question Question)[] questions) =>
        NamedMap(questions, "question");

    /// <summary>
    /// Create a score question using an ordered rubric.
    /// Mirrors JS <c>score()</c>.
    /// </summary>
    /// <param name="instructions">The question as text, a JSON object or array, or <c>null</c>.</param>
    /// <param name="criteria">At least two descriptions indexed by score from zero; entries may be <c>null</c>.</param>
    public static ScoreQuestion Score(object? instructions, IReadOnlyList<object?> criteria)
    {
        if (criteria is IDictionary)
        {
            throw new TypeSafeException(
                "Score criteria must be a list of descriptions indexed by score from zero, not a map.");
        }

        return new ScoreQuestion(instructions, criteria, includeInstructions: true);
    }

    /// <inheritdoc cref="Score(object?, IReadOnlyList{object?})" />
    public static ScoreQuestion Score(object? instructions, params object?[] criteria) =>
        Score(instructions, (IReadOnlyList<object?>)criteria);

    /// <summary>Reject empty question sets and score questions without a list of at least two criteria.</summary>
    public static void Validate(IReadOnlyDictionary<string, Question> questions)
    {
        if (questions.Count == 0)
        {
            throw new TypeSafeException("At least one question is required.");
        }

        foreach (var pair in questions)
        {
            if (pair.Value is not ScoreQuestion score)
            {
                continue;
            }

            if (score.Criteria.Count < 2)
            {
                throw new TypeSafeException(
                    $"Score question \"{pair.Key}\" has {score.Criteria.Count} criteria; at least two scores are required.");
            }
        }
    }

    internal static Dictionary<string, TValue> NamedMap<TValue>(
        (string Key, TValue Value)[]? pairs,
        string itemKind)
    {
        if (pairs is null)
        {
            throw new TypeSafeException($"{itemKind} map must not be null.");
        }

        var map = new Dictionary<string, TValue>(pairs.Length);
        foreach (var (key, value) in pairs)
        {
            if (key is null)
            {
                throw new TypeSafeException($"{itemKind} names must not be null.");
            }

            if (map.ContainsKey(key))
            {
                throw new TypeSafeException($"Duplicate {itemKind} \"{key}\".");
            }

            map[key] = value;
        }

        return map;
    }
}

/// <summary>Optional descriptions of the yes and no outcomes for a Noul question.</summary>
public sealed class NoulCriteria
{
    /// <summary>Description of the yes outcome.</summary>
    public object? True { get; }

    /// <summary>Description of the no outcome.</summary>
    public object? False { get; }

    internal bool IncludeTrue { get; }
    internal bool IncludeFalse { get; }

    /// <summary>Create Noul criteria, omitting unspecified sides on the wire.</summary>
    public NoulCriteria(object? @true = null, object? @false = null, bool includeTrue = true, bool includeFalse = true)
    {
        True = @true;
        False = @false;
        IncludeTrue = includeTrue;
        IncludeFalse = includeFalse;
    }

    /// <summary>Describe only the yes outcome.</summary>
    public static NoulCriteria Yes(object? meaning) => new(meaning, null, includeTrue: true, includeFalse: false);

    /// <summary>Describe only the no outcome.</summary>
    public static NoulCriteria No(object? meaning) => new(null, meaning, includeTrue: false, includeFalse: true);
}

/// <summary>A yes/no question with optional descriptions for either outcome.</summary>
public sealed class NoulQuestion : Question
{
    /// <inheritdoc />
    public override string Type => "noul";

    /// <summary>Optional descriptions of the yes and no outcomes.</summary>
    public NoulCriteria? Criteria { get; }

    internal bool IncludeCriteria { get; }

    /// <summary>Create a Noul question.</summary>
    public NoulQuestion(
        object? instructions = null,
        NoulCriteria? criteria = null,
        bool includeInstructions = true,
        bool includeCriteria = false)
        : base(instructions, includeInstructions)
    {
        Criteria = criteria;
        IncludeCriteria = includeCriteria;
    }
}

/// <summary>A question that selects between named alternatives.</summary>
public sealed class ChoiceQuestion : Question
{
    /// <inheritdoc />
    public override string Type => "choice";

    /// <summary>Labels mapped to descriptions, or <c>null</c> for undescribed labels.</summary>
    public IReadOnlyDictionary<string, object?> Criteria { get; }

    /// <summary>Create a Choice question.</summary>
    public ChoiceQuestion(
        object? instructions,
        IReadOnlyDictionary<string, object?> criteria,
        bool includeInstructions = true)
        : base(instructions, includeInstructions)
    {
        Criteria = criteria ?? throw new TypeSafeException("Choice criteria must be a map of labels to descriptions, not a list.");
    }
}

/// <summary>A question that assigns a score using an ordered rubric.</summary>
public sealed class ScoreQuestion : Question
{
    /// <inheritdoc />
    public override string Type => "score";

    /// <summary>Descriptions of the available outcomes, indexed from zero.</summary>
    public IReadOnlyList<object?> Criteria { get; }

    /// <summary>Create a Score question.</summary>
    public ScoreQuestion(
        object? instructions,
        IReadOnlyList<object?> criteria,
        bool includeInstructions = true)
        : base(instructions, includeInstructions)
    {
        Criteria = criteria ?? throw new TypeSafeException(
            "Score criteria must be a list of descriptions indexed by score from zero, not a map.");
    }
}

internal static class QuestionWire
{
    public static JsonObject ToJson(Question question)
    {
        var node = new JsonObject { ["type"] = question.Type };
        if (question.IncludeInstructions)
        {
            node["instructions"] = JsonValues.ToNode(question.Instructions);
        }

        switch (question)
        {
            case NoulQuestion noul when noul.IncludeCriteria:
                node["criteria"] = noul.Criteria is null ? null : ToNoulCriteria(noul.Criteria);
                break;
            case ChoiceQuestion choice:
                node["criteria"] = ToObject(choice.Criteria);
                break;
            case ScoreQuestion score:
                node["criteria"] = ToArray(score.Criteria);
                break;
        }

        return node;
    }

    private static JsonObject ToNoulCriteria(NoulCriteria criteria)
    {
        var node = new JsonObject();
        if (criteria.IncludeTrue)
        {
            node["true"] = JsonValues.ToNode(criteria.True);
        }

        if (criteria.IncludeFalse)
        {
            node["false"] = JsonValues.ToNode(criteria.False);
        }

        return node;
    }

    private static JsonObject ToObject(IReadOnlyDictionary<string, object?> values)
    {
        var node = new JsonObject();
        foreach (var pair in values)
        {
            node[pair.Key] = JsonValues.ToNode(pair.Value);
        }

        return node;
    }

    private static JsonArray ToArray(IReadOnlyList<object?> values)
    {
        var node = new JsonArray();
        foreach (var value in values)
        {
            node.Add(JsonValues.ToNode(value));
        }

        return node;
    }
}
