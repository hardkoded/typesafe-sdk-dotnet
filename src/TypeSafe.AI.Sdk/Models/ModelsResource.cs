// Copyright (c) Dario Kondratiuk.
// Licensed under the MIT License.

namespace TypeSafe.AI.Sdk;

/// <summary>Access to the Models API resource.</summary>
public sealed class ModelsResource
{
    private readonly TypeSafeClient _client;

    internal ModelsResource(TypeSafeClient client)
    {
        _client = client;
    }

    /// <summary>List the models available to the account.</summary>
    public Task<IReadOnlyList<ModelCard>> ListAsync(
        RequestOptions? options = null,
        CancellationToken cancellationToken = default) =>
        _client.SendAsync(
            HttpMethod.Get,
            "/v1/models",
            body: null,
            options,
            cancellationToken,
            AnswerParser.ParseModels);

    /// <summary>List models and return the HTTP response metadata.</summary>
    public Task<TypeSafeResponse<IReadOnlyList<ModelCard>>> ListWithResponseAsync(
        RequestOptions? options = null,
        CancellationToken cancellationToken = default) =>
        _client.SendWithResponseAsync(
            HttpMethod.Get,
            "/v1/models",
            body: null,
            options,
            cancellationToken,
            AnswerParser.ParseModels);
}
