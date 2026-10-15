// Copyright (c) Dario Kondratiuk.
// Licensed under the MIT License.

using Xunit;

namespace TypeSafe.AI.Sdk.Tests;

public sealed class UsageParsingTests
{
    [Fact]
    public async Task Parses_numeric_token_counts()
    {
        using var client = TestClient.Create(ScriptedHandler.Json(SystemOne(
            new { input_tokens = 12, output_tokens = 34 })));
        var result = await AskAsync(client);

        Assert.Equal<int?>(12, result.Usage.InputTokens);
        Assert.Equal<int?>(34, result.Usage.OutputTokens);
    }

    [Fact]
    public async Task Parses_json_null_token_counts_as_null()
    {
        using var client = TestClient.Create(ScriptedHandler.Json(SystemOne(
            new { input_tokens = (int?)null, output_tokens = (int?)null })));
        var result = await AskAsync(client);

        Assert.Null(result.Usage.InputTokens);
        Assert.Null(result.Usage.OutputTokens);
    }

    [Fact]
    public async Task Parses_missing_token_counts_as_null()
    {
        using var client = TestClient.Create(ScriptedHandler.Json(SystemOne(new { })));
        var result = await AskAsync(client);

        Assert.Null(result.Usage.InputTokens);
        Assert.Null(result.Usage.OutputTokens);
    }

    [Fact]
    public async Task Parses_missing_usage_object_as_null_token_counts()
    {
        using var client = TestClient.Create(ScriptedHandler.Json(new
        {
            model = "m",
            answers = new { q1 = new { type = "noul", noul = 0.5 } },
        }));
        var result = await AskAsync(client);

        Assert.Null(result.Usage.InputTokens);
        Assert.Null(result.Usage.OutputTokens);
    }

    [Fact]
    public async Task Parses_mixed_null_and_numeric_token_counts()
    {
        using var client = TestClient.Create(ScriptedHandler.Json(SystemOne(
            new { input_tokens = 7, output_tokens = (int?)null })));
        var result = await AskAsync(client);

        Assert.Equal<int?>(7, result.Usage.InputTokens);
        Assert.Null(result.Usage.OutputTokens);
    }

    private static object SystemOne(object usage) => new
    {
        model = "m",
        answers = new { q1 = new { type = "noul", noul = 0.5 } },
        usage,
    };

    private static Task<SystemOneResult> AskAsync(TypeSafeClient client) =>
        client.SystemOneAsync("s", new Dictionary<string, Question>
        {
            ["q1"] = Question.Noul("?"),
        });
}
