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
            """{"input_tokens":12,"output_tokens":34}""")));
        var result = await AskAsync(client);

        Assert.Equal<int?>(12, result.Usage.InputTokens);
        Assert.Equal<int?>(34, result.Usage.OutputTokens);
    }

    [Fact]
    public async Task Parses_json_null_token_counts_as_null()
    {
        using var client = TestClient.Create(ScriptedHandler.Json(SystemOne(
            """{"input_tokens":null,"output_tokens":null}""")));
        var result = await AskAsync(client);

        Assert.Null(result.Usage.InputTokens);
        Assert.Null(result.Usage.OutputTokens);
    }

    [Fact]
    public async Task Parses_missing_token_counts_as_null()
    {
        using var client = TestClient.Create(ScriptedHandler.Json(SystemOne("{}")));
        var result = await AskAsync(client);

        Assert.Null(result.Usage.InputTokens);
        Assert.Null(result.Usage.OutputTokens);
    }

    [Fact]
    public async Task Parses_missing_usage_object_as_null_token_counts()
    {
        using var client = TestClient.Create(ScriptedHandler.Json(
            """{"model":"m","answers":{"q1":{"type":"noul","noul":0.5}}}"""));
        var result = await AskAsync(client);

        Assert.Null(result.Usage.InputTokens);
        Assert.Null(result.Usage.OutputTokens);
    }

    [Fact]
    public async Task Parses_mixed_null_and_numeric_token_counts()
    {
        using var client = TestClient.Create(ScriptedHandler.Json(SystemOne(
            """{"input_tokens":7,"output_tokens":null}""")));
        var result = await AskAsync(client);

        Assert.Equal<int?>(7, result.Usage.InputTokens);
        Assert.Null(result.Usage.OutputTokens);
    }

    private static string SystemOne(string usageJson) =>
        $$"""{"model":"m","answers":{"q1":{"type":"noul","noul":0.5}},"usage":{{usageJson}}}""";

    private static Task<SystemOneResult> AskAsync(TypeSafeClient client) =>
        client.SystemOneAsync("s", new Dictionary<string, Question>
        {
            ["q1"] = Question.Noul("?"),
        });
}
