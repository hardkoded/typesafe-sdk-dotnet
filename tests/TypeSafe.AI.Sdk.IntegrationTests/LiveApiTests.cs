// Copyright (c) Hardkoded.
// Licensed under the MIT License.

using Xunit;

namespace TypeSafe.AI.Sdk.IntegrationTests;

/// <summary>
/// Live API tests. Skipped automatically when <c>TYPESAFE_API_KEY</c> is not set
/// so <c>dotnet test</c> stays green in CI without secrets.
/// </summary>
public sealed class LiveApiTests
{
    [LiveApiFact]
    public async Task SystemOne_answers_a_tiny_noul_and_choice()
    {
        using var client = new TypeSafeClient(new TypeSafeClientOptions
        {
            TimeoutMs = 60_000,
            LogLevel = TypeSafeLogLevel.Off,
        });

        var result = await client.SystemOneAsync(
            new { document = "I was charged twice. Please fix this ASAP." },
            ("billing", Question.Noul("Is this about billing?")),
            ("category", Question.Choice("What is this ticket about?",
                ("billing", null),
                ("technical", null),
                ("other", null))));

        Assert.False(string.IsNullOrWhiteSpace(result.Model));
        Assert.InRange(result.GetNoul("billing").Noul, 0, 1);
        Assert.Contains(result.GetChoice("category").Choice, new[] { "billing", "technical", "other" });
        Assert.True(result.Usage.InputTokens > 0);
    }

    [LiveApiFact]
    public async Task Models_list_returns_at_least_one_card()
    {
        using var client = new TypeSafeClient(new TypeSafeClientOptions
        {
            TimeoutMs = 60_000,
            LogLevel = TypeSafeLogLevel.Off,
        });
        var models = await client.Models.ListAsync();
        Assert.NotEmpty(models);
        Assert.False(string.IsNullOrWhiteSpace(models[0].Name));
    }
}

/// <summary>xUnit fact that skips when <c>TYPESAFE_API_KEY</c> is missing.</summary>
public sealed class LiveApiFactAttribute : FactAttribute
{
    public LiveApiFactAttribute()
    {
        if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(Env.ApiKey)))
        {
            Skip = $"Set {Env.ApiKey} to run live API tests.";
        }
    }
}
