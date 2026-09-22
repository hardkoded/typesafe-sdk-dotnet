// Copyright (c) Dario Kondratiuk.
// Licensed under the MIT License.

using System.Text.Json;
using Xunit;

namespace TypeSafe.AI.Sdk.Tests;

public sealed class QuestionAndRequestTests
{
    [Fact]
    public void Choice_accepts_a_criteria_object()
    {
        var question = Question.Choice("q", new Dictionary<string, object?> { ["a"] = "desc", ["b"] = null });
        Assert.Equal("choice", question.Type);
        Assert.Equal("q", question.Instructions);
        Assert.Equal("desc", question.Criteria["a"]);
        Assert.Null(question.Criteria["b"]);
    }

    [Fact]
    public void Choice_accepts_label_description_tuples()
    {
        var question = Question.Choice("q", ("a", "desc"), ("b", null));
        Assert.Equal("choice", question.Type);
        Assert.Equal("desc", question.Criteria["a"]);
        Assert.Null(question.Criteria["b"]);
    }

    [Fact]
    public void Choice_rejects_duplicate_tuple_labels()
    {
        var ex = Assert.Throws<TypeSafeException>(() => Question.Choice("q", ("a", "one"), ("a", "two")));
        Assert.Contains("Duplicate choice label \"a\"", ex.Message);
    }

    [Fact]
    public void Map_builds_a_question_dictionary_and_rejects_duplicate_names()
    {
        var questions = Question.Map(("noul", Question.Noul("x")), ("score", Question.Score("y", "low", "high")));
        Assert.Equal(2, questions.Count);
        Assert.IsType<NoulQuestion>(questions["noul"]);

        var ex = Assert.Throws<TypeSafeException>(() =>
            Question.Map(("q", Question.Noul("a")), ("q", Question.Noul("b"))));
        Assert.Contains("Duplicate question \"q\"", ex.Message);
    }

    [Fact]
    public void Noul_allows_describing_one_side_both_or_neither()
    {
        var plain = Question.Noul("q");
        Assert.Equal("noul", plain.Type);
        Assert.Equal("q", plain.Instructions);
        Assert.False(plain.IncludeCriteria);

        var yesOnly = Question.Noul("q", NoulCriteria.Yes("yes means this"));
        Assert.Equal("yes means this", yesOnly.Criteria!.True);
        Assert.True(yesOnly.Criteria.IncludeTrue);
        Assert.False(yesOnly.Criteria.IncludeFalse);

        var both = Question.Noul("q", new NoulCriteria("a", "b"));
        Assert.Equal("a", both.Criteria!.True);
        Assert.Equal("b", both.Criteria.False);
    }

    [Fact]
    public void Score_keeps_the_list_and_rejects_maps()
    {
        var question = Question.Score("q", "bad", "good");
        Assert.Equal(new object?[] { "bad", "good" }, question.Criteria);
        Assert.Throws<TypeSafeException>(() => Question.Score("q", (IReadOnlyList<object?>)null!));
    }

    [Fact]
    public void Descriptions_can_be_json_objects()
    {
        var rich = new Dictionary<string, object?> { ["summary"] = "warm", ["examples"] = new object[] { "hi!", "welcome" } };
        Assert.Same(rich, Question.Choice("q", new Dictionary<string, object?> { ["friendly"] = rich, ["hostile"] = null }).Criteria["friendly"]);
        Assert.Same(rich, Question.Score("q", rich, "meh").Criteria[0]);
        Assert.Same(rich, Question.Noul("q", NoulCriteria.Yes(rich)).Criteria!.True);
    }

    [Fact]
    public async Task Posts_systemOne_payload_with_the_default_model()
    {
        var handler = ScriptedHandler.Json(Fixtures.SystemOne);
        using var client = TestClient.Create(handler, o => o.BaseUrl = "https://x.test");
        var result = await client.SystemOneAsync(new Dictionary<string, object?> { ["a"] = 1 }, new Dictionary<string, Question>
        {
            ["q1"] = Question.Noul("x"),
        });

        Assert.Equal("https://x.test/v1/systemone", handler.Requests[0].Url);
        Assert.Equal("POST", handler.Requests[0].Method);
        var body = handler.Requests[0].Body!.Value;
        Assert.Equal("jev-latest", body.GetProperty("model").GetString());
        Assert.Equal(1, body.GetProperty("state").GetProperty("a").GetInt32());
        Assert.Equal("noul", body.GetProperty("questions").GetProperty("q1").GetProperty("type").GetString());
        Assert.Equal("x", body.GetProperty("questions").GetProperty("q1").GetProperty("instructions").GetString());
        Assert.False(body.GetProperty("questions").GetProperty("q1").TryGetProperty("criteria", out _));
        Assert.Equal(0.5, result.GetNoul("q1").Noul);
    }

    [Fact]
    public async Task Posts_systemOne_payload_from_question_tuples()
    {
        var handler = ScriptedHandler.Json(Fixtures.SystemOne);
        using var client = TestClient.Create(handler, o => o.BaseUrl = "https://x.test");
        var result = await client.SystemOneAsync(
            new Dictionary<string, object?> { ["a"] = 1 },
            ("q1", Question.Noul("x")),
            ("choice", Question.Choice("which?", ("yes", null), ("no", "negatory"))));

        var body = handler.Requests[0].Body!.Value;
        Assert.Equal("jev-latest", body.GetProperty("model").GetString());
        Assert.Equal(1, body.GetProperty("state").GetProperty("a").GetInt32());
        Assert.Equal("noul", body.GetProperty("questions").GetProperty("q1").GetProperty("type").GetString());
        Assert.Equal("choice", body.GetProperty("questions").GetProperty("choice").GetProperty("type").GetString());
        Assert.Equal(JsonValueKind.Null, body.GetProperty("questions").GetProperty("choice").GetProperty("criteria").GetProperty("yes").ValueKind);
        Assert.Equal("negatory", body.GetProperty("questions").GetProperty("choice").GetProperty("criteria").GetProperty("no").GetString());
        Assert.Equal(0.5, result.GetNoul("q1").Noul);
    }

    [Fact]
    public async Task Honors_per_call_model_and_client_default_model()
    {
        var handler = ScriptedHandler.Json(Fixtures.SystemOne);
        using var client = TestClient.Create(handler, o => o.DefaultModel = "client-default");
        await client.SystemOneAsync("s", new Dictionary<string, Question>
        {
            ["q"] = Question.Choice("c", new Dictionary<string, object?> { ["a"] = null }),
        });
        await client.SystemOneAsync("s", new Dictionary<string, Question>
        {
            ["q"] = Question.Choice("c", new Dictionary<string, object?> { ["a"] = null }),
        }, model: "per-call");
        Assert.Equal("client-default", handler.Requests[0].Body!.Value.GetProperty("model").GetString());
        Assert.Equal("per-call", handler.Requests[1].Body!.Value.GetProperty("model").GetString());
    }

    [Fact]
    public async Task Preserves_null_state_instructions_and_criteria()
    {
        var handler = ScriptedHandler.Json(Fixtures.SystemOne);
        using var client = TestClient.Create(handler);
        await client.SystemOneAsync(new SystemOneRequest
        {
            State = null,
            Questions = new Dictionary<string, Question>
            {
                ["noul"] = Question.Noul(null, new NoulCriteria(null, null)),
                ["noCriteria"] = Question.Noul(null, null, includeCriteria: true),
                ["choice"] = Question.Choice(null, new Dictionary<string, object?> { ["yes"] = null, ["no"] = null }),
                ["score"] = Question.Score(null, new object?[] { null, "high" }),
            },
        });

        var questions = handler.Requests[0].Body!.Value.GetProperty("questions");
        Assert.Equal(JsonValueKind.Null, handler.Requests[0].Body!.Value.GetProperty("state").ValueKind);
        Assert.Equal(JsonValueKind.Null, questions.GetProperty("noul").GetProperty("instructions").ValueKind);
        Assert.Equal(JsonValueKind.Null, questions.GetProperty("noul").GetProperty("criteria").GetProperty("true").ValueKind);
        Assert.Equal(JsonValueKind.Null, questions.GetProperty("noCriteria").GetProperty("criteria").ValueKind);
        Assert.Equal(JsonValueKind.Null, questions.GetProperty("choice").GetProperty("criteria").GetProperty("yes").ValueKind);
        Assert.Equal(JsonValueKind.Null, questions.GetProperty("score").GetProperty("criteria")[0].ValueKind);
        Assert.Equal("high", questions.GetProperty("score").GetProperty("criteria")[1].GetString());
    }

    [Fact]
    public async Task Sends_score_list_and_choice_map_untouched()
    {
        var handler = ScriptedHandler.Json(Fixtures.SystemOne);
        using var client = TestClient.Create(handler);
        await client.SystemOneAsync("s", new Dictionary<string, Question>
        {
            ["choice"] = Question.Choice("which?", new Dictionary<string, object?> { ["a"] = null, ["b"] = null }),
            ["score"] = Question.Score("q", "bad", "ok", "great"),
        });
        var questions = handler.Requests[0].Body!.Value.GetProperty("questions");
        Assert.Equal(JsonValueKind.Null, questions.GetProperty("choice").GetProperty("criteria").GetProperty("a").ValueKind);
        Assert.Equal(new[] { "bad", "ok", "great" }, questions.GetProperty("score").GetProperty("criteria").EnumerateArray().Select(x => x.GetString()).ToArray());
    }

    [Fact]
    public void Rejects_bad_score_criteria_and_empty_question_sets_before_sending()
    {
        var handler = ScriptedHandler.Json(Fixtures.SystemOne);
        using var client = TestClient.Create(handler);
        var empty = Assert.Throws<TypeSafeException>(() =>
            client.SystemOneAsync("s", new Dictionary<string, Question>()).GetAwaiter().GetResult());
        Assert.Contains("At least one question is required", empty.Message);

        var shortScore = Assert.Throws<TypeSafeException>(() =>
            client.SystemOneAsync("s", new Dictionary<string, Question>
            {
                ["q"] = Question.Score("?", new object?[] { "only" }),
            }).GetAwaiter().GetResult());
        Assert.Contains("at least two scores", shortScore.Message);
        Assert.Empty(handler.Requests);
    }

    [Fact]
    public async Task Choice_answers_carry_the_winning_label()
    {
        var handler = ScriptedHandler.Json(new
        {
            model = "m",
            answers = new
            {
                q = new
                {
                    type = "choice",
                    choice = "b",
                    confidence = 0.9,
                    probabilities = new { a = 0.1, b = 0.9 },
                },
            },
            usage = new { input_tokens = 1, output_tokens = 1 },
        });
        using var client = TestClient.Create(handler);
        var result = await client.SystemOneAsync("s", new Dictionary<string, Question>
        {
            ["q"] = Question.Choice("q", new Dictionary<string, object?> { ["a"] = null, ["b"] = null }),
        });
        Assert.Equal("b", result.GetChoice("q").Choice);
        Assert.Equal(0.9, result.GetChoice("q").Confidence);
        Assert.Equal(0.1, result.GetChoice("q").Probabilities["a"]);
    }

    [Fact]
    public async Task Forwards_extra_fields_and_null()
    {
        var handler = ScriptedHandler.Json(Fixtures.SystemOne);
        using var client = TestClient.Create(handler);
        await client.SystemOneAsync(new SystemOneRequest
        {
            State = "s",
            Questions = new Dictionary<string, Question> { ["q"] = Question.Score("?", "low", "high") },
            Extra = new Dictionary<string, object?>
            {
                ["future_option"] = null,
                ["nested"] = new Dictionary<string, object?> { ["enabled"] = true },
            },
        });
        var body = handler.Requests[0].Body!.Value;
        Assert.Equal(JsonValueKind.Null, body.GetProperty("future_option").ValueKind);
        Assert.True(body.GetProperty("nested").GetProperty("enabled").GetBoolean());
    }

    [Fact]
    public async Task Models_list_unwraps_documented_response()
    {
        var handler = ScriptedHandler.Json(Fixtures.Models);
        using var client = TestClient.Create(handler);
        var models = await client.Models.ListAsync();
        Assert.Single(models);
        Assert.Equal("m", models[0].Name);
        Assert.Equal("d", models[0].Description);
        Assert.Equal("2026", models[0].ReleaseDate);

        var wrapped = await client.Models.ListWithResponseAsync();
        Assert.Equal("m", wrapped.Data[0].Name);
        Assert.Equal(System.Net.HttpStatusCode.OK, wrapped.Response.StatusCode);
    }

    [Theory]
    [InlineData("null")]
    [InlineData("[]")]
    [InlineData("{\"models\":null}")]
    [InlineData("{\"ok\":true}")]
    public async Task Models_list_fails_clearly_on_unrecognized_shape(string wire)
    {
        var handler = new ScriptedHandler((_, _) => ScriptedHandler.JsonResponse(wire));
        using var client = TestClient.Create(handler);
        var ex = await Assert.ThrowsAsync<TypeSafeException>(() => client.Models.ListAsync());
        Assert.Contains("Unexpected response shape from GET /v1/models", ex.Message);
    }

    [Fact]
    public async Task Sends_auth_and_identifying_headers()
    {
        var handler = ScriptedHandler.Json(new { models = Array.Empty<object>() });
        using var client = TestClient.Create(handler, o =>
        {
            o.ApiKey = "secret";
            o.BaseUrl = "https://x.test";
        });
        await client.Models.ListAsync();
        var headers = handler.Requests[0].Headers;
        Assert.Equal("https://x.test/v1/models", handler.Requests[0].Url);
        Assert.Equal("GET", handler.Requests[0].Method);
        Assert.Equal("Bearer secret", headers["Authorization"]);
        Assert.StartsWith("typesafe-sdk-dotnet/", headers["User-Agent"]);
        Assert.Equal(headers["User-Agent"], headers["X-TypeSafe-SDK"]);
        Assert.StartsWith("dotnet/", headers["X-TypeSafe-Runtime"]);
        Assert.False(headers.ContainsKey("X-TypeSafe-Retry-Count"));
    }

    [Fact]
    public async Task Merges_headers_without_clobbering_auth()
    {
        var handler = ScriptedHandler.Json(new { models = Array.Empty<object>() });
        using var client = TestClient.Create(handler, o =>
        {
            o.ApiKey = "secret";
            o.DefaultHeaders = new Dictionary<string, string>
            {
                ["X-Trace"] = "client",
                ["X-Only-Default"] = "yes",
                ["Authorization"] = "nope",
            };
        });
        await client.Models.ListAsync(new RequestOptions
        {
            Headers = new Dictionary<string, string>
            {
                ["X-Trace"] = "call",
                ["X-Only-Call"] = "yes",
            },
        });
        var headers = handler.Requests[0].Headers;
        Assert.Equal("call", headers["X-Trace"]);
        Assert.Equal("yes", headers["X-Only-Default"]);
        Assert.Equal("yes", headers["X-Only-Call"]);
        Assert.Equal("Bearer secret", headers["Authorization"]);
    }

    [Fact]
    public async Task WithResponse_exposes_request_id()
    {
        var handler = new ScriptedHandler((_, _) => ScriptedHandler.JsonResponse(
            Fixtures.SystemOne,
            headers: new Dictionary<string, string> { ["x-typesafe-request-id"] = "req_1" }));
        using var client = TestClient.Create(handler);
        var result = await client.SystemOneWithResponseAsync("s", new Dictionary<string, Question>
        {
            ["q1"] = Question.Noul("?"),
        });
        Assert.Equal("req_1", result.RequestId);
        Assert.Equal("m", result.Data.Model);
        Assert.Equal(10_000, client.TimeoutMs);
    }
}
