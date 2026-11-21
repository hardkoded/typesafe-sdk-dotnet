// Copyright (c) Dario Kondratiuk.
// Licensed under the MIT License.

using Xunit;

namespace TypeSafe.AI.Sdk.Tests;

public sealed class AnswerParsingTests
{
    private static Dictionary<string, Question> Ask() => new() { ["q"] = Question.Noul("?") };

    private static async Task<SystemOneResult> ParseAsync(object body)
    {
        using var client = TestClient.Create(ScriptedHandler.Json(body));
        return await client.SystemOneAsync("s", Ask());
    }

    private static async Task<TypeSafeException> RejectAsync(object answer)
    {
        using var client = TestClient.Create(
            ScriptedHandler.Json(new { model = "m", answers = new { q = answer } })
        );
        return await Assert.ThrowsAsync<TypeSafeException>(() => client.SystemOneAsync("s", Ask()));
    }

    [Fact]
    public async Task Score_question_with_one_level_is_sent_as_a_one_item_array()
    {
        var handler = ScriptedHandler.Json(
            new { model = "m", answers = new { q = new { type = "noul", noul = 0.5 } } }
        );
        using var client = TestClient.Create(handler);

        await client.SystemOneAsync(
            "s",
            new Dictionary<string, Question>
            {
                ["q"] = Question.Score("?", new object?[] { "only" }),
            }
        );

        var criteria = handler.Requests[0].Body!.Value.GetProperty("questions").GetProperty("q");
        Assert.Equal(1, criteria.GetProperty("criteria").GetArrayLength());
    }

    [Fact]
    public async Task Null_or_missing_confidence_is_reported_as_null_on_choice_and_score()
    {
        var result = await ParseAsync(
            new
            {
                model = "m",
                answers = new
                {
                    a = new
                    {
                        type = "choice",
                        choice = "x",
                        confidence = (double?)null,
                        probabilities = new { x = 1.0 },
                    },
                    b = new
                    {
                        type = "choice",
                        choice = "x",
                        probabilities = new { x = 1.0 },
                    },
                    c = new
                    {
                        type = "score",
                        score = 1.5,
                        confidence = (double?)null,
                        probabilities = new { x = 1.0 },
                    },
                    d = new
                    {
                        type = "score",
                        score = 1.5,
                        probabilities = new { x = 1.0 },
                    },
                    e = new
                    {
                        type = "choice",
                        choice = "x",
                        confidence = 0.8,
                        probabilities = new { x = 1.0 },
                    },
                },
            }
        );

        Assert.Null(result.GetChoice("a").ReportedConfidence);
        Assert.Equal(0, result.GetChoice("a").Confidence);
        Assert.Null(result.GetChoice("b").ReportedConfidence);
        Assert.Null(result.GetScore("c").ReportedConfidence);
        Assert.Equal(0, result.GetScore("c").Confidence);
        Assert.Null(result.GetScore("d").ReportedConfidence);
        Assert.Equal(0.8, result.GetChoice("e").ReportedConfidence);
        Assert.Equal(0.8, result.GetChoice("e").Confidence);
    }

    [Fact]
    public async Task Null_model_and_usage_parse_without_inventing_token_counts()
    {
        var result = await ParseAsync(
            new
            {
                model = (string?)null,
                usage = (object?)null,
                answers = new { q = new { type = "noul", noul = 0.5 } },
            }
        );

        Assert.Equal("", result.Model);
        Assert.Null(result.Usage.InputTokens);
        Assert.Null(result.Usage.OutputTokens);
        Assert.Equal(0.5, result.GetNoul("q").Noul);
    }

    [Fact]
    public async Task Noul_answer_without_a_number_is_rejected()
    {
        var error = await RejectAsync(new { type = "noul" });
        Assert.Contains("\"q\"", error.Message);
        Assert.Contains("\"noul\"", error.Message);
        await RejectAsync(new { type = "noul", noul = "yes" });
    }

    [Fact]
    public async Task Choice_answer_requires_choice_and_probabilities()
    {
        var missingChoice = await RejectAsync(
            new { type = "choice", probabilities = new { a = 1.0 } }
        );
        Assert.Contains("\"choice\"", missingChoice.Message);
        await RejectAsync(
            new
            {
                type = "choice",
                choice = 3,
                probabilities = new { a = 1.0 },
            }
        );

        var missingProbabilities = await RejectAsync(new { type = "choice", choice = "a" });
        Assert.Contains("\"probabilities\"", missingProbabilities.Message);
        await RejectAsync(
            new
            {
                type = "choice",
                choice = "a",
                probabilities = new { a = "high" },
            }
        );
    }

    [Fact]
    public async Task Score_answer_requires_score_and_probabilities_but_legend_may_be_null()
    {
        await RejectAsync(new { type = "score", probabilities = new { a = 1.0 } });
        await RejectAsync(
            new
            {
                type = "score",
                score = "high",
                probabilities = new { a = 1.0 },
            }
        );
        await RejectAsync(new { type = "score", score = 1.0 });

        var result = await ParseAsync(
            new
            {
                model = "m",
                answers = new
                {
                    q = new
                    {
                        type = "score",
                        score = 1.0,
                        legend = (object?)null,
                        probabilities = new { a = 1.0 },
                    },
                },
            }
        );
        Assert.Empty(result.GetScore("q").Legend);
    }

    [Fact]
    public async Task Box_answer_requires_a_box_with_four_numeric_edges()
    {
        await RejectAsync(new { type = "box" });
        await RejectAsync(
            new
            {
                type = "box",
                box = new
                {
                    top = 0.1,
                    bottom = 0.2,
                    left = 0.3,
                },
            }
        );
        await RejectAsync(
            new
            {
                type = "box",
                box = new
                {
                    top = 0.1,
                    bottom = 0.2,
                    left = 0.3,
                    right = "x",
                },
            }
        );
    }

    [Fact]
    public async Task Unknown_types_and_non_object_answers_are_rejected()
    {
        await RejectAsync(new { type = "mystery" });

        using var client = TestClient.Create(ScriptedHandler.Json(new { model = "m" }));
        await Assert.ThrowsAsync<TypeSafeException>(() => client.SystemOneAsync("s", Ask()));
    }
}
