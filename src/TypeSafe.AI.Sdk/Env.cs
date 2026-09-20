namespace TypeSafe.AI.Sdk;

/// <summary>
/// Environment variable names used when constructing <see cref="TypeSafeClient"/>.
/// Explicit constructor options take precedence over these values.
/// Empty or whitespace-only values are ignored.
/// </summary>
public static class Env
{
    /// <summary>Required API key; used when <see cref="TypeSafeClientOptions.ApiKey"/> is omitted.</summary>
    public const string ApiKey = "TYPESAFE_API_KEY";

    /// <summary>API root; defaults to <c>https://api.typesafe.ai</c>.</summary>
    public const string BaseUrl = "TYPESAFE_BASE_URL";

    /// <summary>Default model name; defaults to <c>jev-latest</c>.</summary>
    public const string DefaultModel = "TYPESAFE_DEFAULT_MODEL";

    /// <summary>Log level; defaults to <c>warn</c>.</summary>
    public const string LogLevel = "TYPESAFE_LOG_LEVEL";

    /// <summary>Read a trimmed environment value, returning <c>null</c> for missing or blank values.</summary>
    public static string? Read(string name)
    {
        var value = Environment.GetEnvironmentVariable(name);
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return value.Trim();
    }

    /// <summary>Return the explicit value, falling back to the environment.</summary>
    public static string? FromCodeOrEnv(string? fromCode, string envVar) =>
        fromCode ?? Read(envVar);
}
