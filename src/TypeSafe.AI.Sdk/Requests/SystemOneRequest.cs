// Copyright (c) Dario Kondratiuk.
// Licensed under the MIT License.

namespace TypeSafe.AI.Sdk;

/// <summary>State and named questions for <see cref="TypeSafeClient.SystemOneAsync(SystemOneRequest, RequestOptions?, CancellationToken)"/>.</summary>
public sealed class SystemOneRequest
{
    /// <summary>Text, a JSON object or array, or <c>null</c> to evaluate.</summary>
    public object? State { get; set; }

    /// <summary>Nonempty questions keyed by the names used to identify their answers.</summary>
    public IDictionary<string, Question> Questions { get; set; } = new Dictionary<string, Question>();

    /// <summary>Model override; omitted values inherit the client <c>defaultModel</c>.</summary>
    public string? Model { get; set; }

    /// <summary>
    /// Additional properties forwarded on the wire, including <c>null</c> values.
    /// Matches the JS SDK's extra-field forwarding.
    /// </summary>
    public IDictionary<string, object?>? Extra { get; set; }
}

/// <summary>Per-call options that override client settings.</summary>
public sealed class RequestOptions
{
    /// <summary>Timeout per attempt in milliseconds; there is no total retry budget.</summary>
    public int? TimeoutMs { get; set; }

    /// <summary>Retry overrides for this call; omitted fields inherit client settings.</summary>
    public RetryPolicy? Retry { get; set; }

    /// <summary>Additional headers, merged over <see cref="TypeSafeClientOptions.DefaultHeaders"/>.</summary>
    public IDictionary<string, string>? Headers { get; set; }

    /// <summary>Cancellation token for the request and pending retries.</summary>
    public CancellationToken CancellationToken { get; set; }
}

/// <summary>Client options. Explicit values take precedence over environment variables, then SDK defaults.</summary>
public sealed class TypeSafeClientOptions
{
    /// <summary>Required API key; falls back to <c>TYPESAFE_API_KEY</c>.</summary>
    public string? ApiKey { get; set; }

    /// <summary>API root; falls back to <c>TYPESAFE_BASE_URL</c>, then <c>https://api.typesafe.ai</c>.</summary>
    public string? BaseUrl { get; set; }

    /// <summary>Default model; falls back to <c>TYPESAFE_DEFAULT_MODEL</c>, then <c>jev-latest</c>.</summary>
    public string? DefaultModel { get; set; }

    /// <summary>
    /// Log level; falls back to <c>TYPESAFE_LOG_LEVEL</c>, then <c>warn</c>.
    /// <c>info</c> logs request summaries; <c>debug</c> adds headers and bodies.
    /// Known credential headers are redacted; bodies are not.
    /// </summary>
    public TypeSafeLogLevel? LogLevel { get; set; }

    /// <summary>Raw log-level string (for env-style values such as <c>off</c>).</summary>
    public string? LogLevelText { get; set; }

    /// <summary>Logger filtered to <see cref="LogLevel"/> and above. Default: prefixed console.</summary>
    public ITypeSafeLogger? Logger { get; set; }

    /// <summary>Retry overrides; omitted fields use the defaults in <see cref="RetryPolicy"/>.</summary>
    public RetryPolicy? Retry { get; set; }

    /// <summary>Timeout per attempt in milliseconds, without a total retry budget. Default: 10000.</summary>
    public int? TimeoutMs { get; set; }

    /// <summary>Additional request headers; per-call headers take precedence.</summary>
    public IDictionary<string, string>? DefaultHeaders { get; set; }

    /// <summary>
    /// Request body compression, sent as <c>Content-Encoding</c>. Default:
    /// <see cref="RequestCompression.Auto"/>, which uses zstd when available, otherwise
    /// <see cref="RequestCompression.None"/>. Bodies under 1 KiB are always sent uncompressed.
    /// Pass <see cref="RequestCompression.None"/> to send all bodies uncompressed, for example
    /// to an API that doesn't accept compressed requests. Explicit
    /// <see cref="RequestCompression.Zstd"/> throws <see cref="TypeSafeException"/> if zstd
    /// is unavailable.
    /// </summary>
    /// <remarks>
    /// zstd is provided by ZstdSharp.Port on both <c>net10.0</c> and <c>netstandard2.0</c>.
    /// The BCL has no zstd APIs until .NET 11 (<c>System.IO.Compression.ZstandardStream</c>).
    /// </remarks>
    public RequestCompression Compression { get; set; }

    /// <summary>
    /// Optional <see cref="HttpMessageHandler"/> used when the client creates its own <see cref="HttpClient"/>.
    /// Useful for tests.
    /// </summary>
    public HttpMessageHandler? HttpMessageHandler { get; set; }
}
