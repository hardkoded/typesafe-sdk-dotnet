// Copyright (c) Dario Kondratiuk.
// Licensed under the MIT License.

using Xunit;

namespace TypeSafe.AI.Sdk.Tests;

[Collection("Env")]
public sealed class ClientConfigurationTests
{
    [Fact]
    public void Falls_back_to_defaults_when_neither_config_nor_env_is_set()
    {
        using var env = EnvScope.Clear();
        using var client = new TypeSafeClient(new TypeSafeClientOptions { ApiKey = "k" });
        Assert.Equal(TypeSafeClient.DefaultBaseUrl, client.BaseUrl);
        Assert.Equal(TypeSafeClient.DefaultModelName, client.DefaultModel);
        Assert.Equal(TypeSafeLogLevel.Warn, client.LogLevel);
        Assert.Equal(2, client.Retry.MaxRetries);
        Assert.Equal(500, client.Retry.BackoffInitialMs);
        Assert.Equal(5000, client.Retry.BackoffMaxMs);
        Assert.Equal(0.25, client.Retry.BackoffJitter);
        Assert.True(client.Retry.HttpStatuses.Contains(408));
        Assert.True(client.Retry.HttpStatuses.Contains(429));
        Assert.True(client.Retry.HttpStatuses.Contains(529));
        Assert.Equal(10_000, client.TimeoutMs);
    }

    [Fact]
    public void Reads_every_setting_from_the_environment()
    {
        using var env = EnvScope.Clear();
        env.Set(Env.ApiKey, "env-key");
        env.Set(Env.BaseUrl, "https://env.test");
        env.Set(Env.DefaultModel, "env-model");
        env.Set(Env.LogLevel, "debug");
        using var client = new TypeSafeClient();
        Assert.Equal("https://env.test", client.BaseUrl);
        Assert.Equal("env-model", client.DefaultModel);
        Assert.Equal(TypeSafeLogLevel.Debug, client.LogLevel);
    }

    [Fact]
    public void Prefers_config_over_the_environment()
    {
        using var env = EnvScope.Clear();
        env.Set(Env.ApiKey, "env-key");
        env.Set(Env.BaseUrl, "https://env.test");
        env.Set(Env.DefaultModel, "env-model");
        env.Set(Env.LogLevel, "debug");
        using var client = new TypeSafeClient(new TypeSafeClientOptions
        {
            ApiKey = "code-key",
            BaseUrl = "https://code.test",
            DefaultModel = "code-model",
            LogLevel = TypeSafeLogLevel.Error,
        });
        Assert.Equal("https://code.test", client.BaseUrl);
        Assert.Equal("code-model", client.DefaultModel);
        Assert.Equal(TypeSafeLogLevel.Error, client.LogLevel);
    }

    [Fact]
    public void Keeps_the_api_key_off_public_properties()
    {
        using var env = EnvScope.Clear();
        using var client = new TypeSafeClient(new TypeSafeClientOptions { ApiKey = "super-secret" });
        Assert.Null(client.GetType().GetProperty("ApiKey"));
        Assert.DoesNotContain("super-secret", client.BaseUrl);
        Assert.DoesNotContain("super-secret", $"{client.DefaultModel}{client.TimeoutMs}");
    }

    [Fact]
    public void Treats_empty_and_whitespace_env_values_as_unset()
    {
        using var env = EnvScope.Clear();
        env.Set(Env.ApiKey, "k");
        env.Set(Env.BaseUrl, " ");
        env.Set(Env.DefaultModel, "");
        env.Set(Env.LogLevel, "");
        using var client = new TypeSafeClient();
        Assert.Equal(TypeSafeClient.DefaultBaseUrl, client.BaseUrl);
        Assert.Equal(TypeSafeClient.DefaultModelName, client.DefaultModel);
        Assert.Equal(TypeSafeLogLevel.Warn, client.LogLevel);
    }

    [Fact]
    public void Throws_naming_the_env_var_when_no_api_key_is_available()
    {
        using var env = EnvScope.Clear();
        var ex = Assert.Throws<TypeSafeException>(() => new TypeSafeClient());
        Assert.Contains(Env.ApiKey, ex.Message);
    }

    [Fact]
    public void Strips_trailing_slashes_from_base_url()
    {
        using var env = EnvScope.Clear();
        env.Set(Env.BaseUrl, "https://example.test///");
        Assert.Equal("https://example.test", new TypeSafeClient(new TypeSafeClientOptions { ApiKey = "k" }).BaseUrl);
        Assert.Equal("https://x.test", new TypeSafeClient(new TypeSafeClientOptions { ApiKey = "k", BaseUrl = "https://x.test/" }).BaseUrl);
    }

    [Theory]
    [InlineData("debug", TypeSafeLogLevel.Debug)]
    [InlineData("info", TypeSafeLogLevel.Info)]
    [InlineData("warn", TypeSafeLogLevel.Warn)]
    [InlineData("error", TypeSafeLogLevel.Error)]
    [InlineData("off", TypeSafeLogLevel.Off)]
    public void Accepts_log_level(string name, TypeSafeLogLevel expected)
    {
        using var env = EnvScope.Clear();
        using var fromCode = new TypeSafeClient(new TypeSafeClientOptions { ApiKey = "k", LogLevelText = name });
        Assert.Equal(expected, fromCode.LogLevel);
        env.Set(Env.LogLevel, name);
        using var fromEnv = new TypeSafeClient(new TypeSafeClientOptions { ApiKey = "k" });
        Assert.Equal(expected, fromEnv.LogLevel);
    }

    [Fact]
    public void Rejects_an_invalid_log_level_and_names_where_it_came_from()
    {
        using var env = EnvScope.Clear();
        env.Set(Env.LogLevel, "loud");
        var fromEnv = Assert.Throws<TypeSafeException>(() => new TypeSafeClient(new TypeSafeClientOptions { ApiKey = "k" }));
        Assert.Contains($"\"loud\" from {Env.LogLevel}", fromEnv.Message);
        Assert.Contains("debug, info, warn, error, off", fromEnv.Message);

        var fromCode = Assert.Throws<TypeSafeException>(() =>
            new TypeSafeClient(new TypeSafeClientOptions { ApiKey = "k", LogLevelText = "loud" }));
        Assert.Contains("the `logLevel` option", fromCode.Message);
    }
}

internal sealed class EnvScope : IDisposable
{
    private readonly Dictionary<string, string?> _previous = new();

    public static EnvScope Clear()
    {
        var scope = new EnvScope();
        foreach (var name in new[] { Env.ApiKey, Env.BaseUrl, Env.DefaultModel, Env.LogLevel })
        {
            scope._previous[name] = Environment.GetEnvironmentVariable(name);
            Environment.SetEnvironmentVariable(name, null);
        }

        return scope;
    }

    public void Set(string name, string? value) => Environment.SetEnvironmentVariable(name, value);

    public void Dispose()
    {
        foreach (var pair in _previous)
        {
            Environment.SetEnvironmentVariable(pair.Key, pair.Value);
        }
    }
}
