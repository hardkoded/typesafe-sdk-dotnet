// Copyright (c) Dario Kondratiuk.
// Licensed under the MIT License.

namespace TypeSafe.AI.Sdk;

/// <summary>Client for the TypeSafe AI API.</summary>
public interface ITypeSafeClient
{
    /// <summary>API root with trailing slashes removed.</summary>
    string BaseUrl { get; }

    /// <summary>Model used when a request omits <c>model</c>.</summary>
    string DefaultModel { get; }

    /// <summary>Configured log verbosity.</summary>
    TypeSafeLogLevel LogLevel { get; }

    /// <summary>Retry settings with constructor overrides applied.</summary>
    RetryPolicy Retry { get; }

    /// <summary>Timeout per attempt in milliseconds.</summary>
    int TimeoutMs { get; }

    /// <summary>The models available to the account.</summary>
    ModelsResource Models { get; }

    /// <summary>Answer named questions about text or structured state.</summary>
    Task<SystemOneResult> SystemOneAsync(
        SystemOneRequest request,
        RequestOptions? options = null,
        CancellationToken cancellationToken = default);

    /// <summary>Answer named questions and return the HTTP response metadata.</summary>
    Task<TypeSafeResponse<SystemOneResult>> SystemOneWithResponseAsync(
        SystemOneRequest request,
        RequestOptions? options = null,
        CancellationToken cancellationToken = default);

    /// <summary>Answer named questions about text or structured state.</summary>
    Task<SystemOneResult> SystemOneAsync(
        object? state,
        IDictionary<string, Question> questions,
        string? model = null,
        RequestOptions? options = null,
        CancellationToken cancellationToken = default);

    /// <summary>Answer named questions and return the HTTP response metadata.</summary>
    Task<TypeSafeResponse<SystemOneResult>> SystemOneWithResponseAsync(
        object? state,
        IDictionary<string, Question> questions,
        string? model = null,
        RequestOptions? options = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Answer named questions about text or structured state.
    /// Duplicate names throw. Use the dictionary overload for <c>model</c>, options, or a cancellation token.
    /// </summary>
    Task<SystemOneResult> SystemOneAsync(
        object? state,
        params (string Name, Question Question)[] questions);

    /// <summary>
    /// Answer named questions and return the HTTP response metadata.
    /// Duplicate names throw. Use the dictionary overload for <c>model</c>, options, or a cancellation token.
    /// </summary>
    Task<TypeSafeResponse<SystemOneResult>> SystemOneWithResponseAsync(
        object? state,
        params (string Name, Question Question)[] questions);
}
