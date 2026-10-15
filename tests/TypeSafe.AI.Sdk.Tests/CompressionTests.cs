// Copyright (c) Dario Kondratiuk.
// Licensed under the MIT License.

using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;
using ZstdSharp;

namespace TypeSafe.AI.Sdk.Tests;

public sealed class CompressionTests
{
    private static Task<SystemOneResult> SendAsync(TypeSafeClient client, string state) =>
        client.SystemOneAsync(state, new Dictionary<string, Question> { ["q1"] = Question.Noul("x") });

    [Fact]
    public async Task Large_bodies_are_compressed_with_zstd()
    {
        var state = new string('a', 5000);
        var handler = ScriptedHandler.Json(Fixtures.SystemOne);
        using var client = TestClient.Create(handler);

        await SendAsync(client, state);

        var request = Assert.Single(handler.Requests);
        Assert.Equal("zstd", request.Headers["Content-Encoding"]);
        Assert.StartsWith("application/json", request.Headers["Content-Type"]);
        var compressed = request.BodyBytes!;
        Assert.True(compressed.Length < 1024);
        using var decompressor = new Decompressor();
        var json = JsonDocument.Parse(Encoding.UTF8.GetString(decompressor.Unwrap(compressed)));
        Assert.Equal(state, json.RootElement.GetProperty("state").GetString());
    }

    [Fact]
    public async Task Bodies_under_1_KiB_are_sent_uncompressed()
    {
        var handler = ScriptedHandler.Json(Fixtures.SystemOne);
        using var client = TestClient.Create(handler);

        await SendAsync(client, "short");

        var request = Assert.Single(handler.Requests);
        Assert.False(request.Headers.ContainsKey("Content-Encoding"));
        Assert.Equal("short", request.Body!.Value.GetProperty("state").GetString());
    }

    [Fact]
    public void The_threshold_is_measured_in_UTF8_bytes()
    {
        Assert.False(BodyCompression.Encode(new string('a', 1023), compression: true).Compressed);
        Assert.True(BodyCompression.Encode(new string('a', 1024), compression: true).Compressed);
        Assert.True(BodyCompression.Encode(new string('é', 600), compression: true).Compressed);
    }

    [Fact]
    public async Task Compression_can_be_disabled()
    {
        var state = new string('a', 5000);
        var handler = ScriptedHandler.Json(Fixtures.SystemOne);
        using var client = TestClient.Create(handler, o => o.Compression = false);

        await SendAsync(client, state);

        var request = Assert.Single(handler.Requests);
        Assert.False(client.Compression);
        Assert.False(request.Headers.ContainsKey("Content-Encoding"));
        Assert.Equal(state, request.Body!.Value.GetProperty("state").GetString());
    }

    [Fact]
    public async Task Compression_is_on_by_default_and_cannot_be_overridden_by_a_user_header()
    {
        var handler = ScriptedHandler.Json(Fixtures.SystemOne);
        using var client = TestClient.Create(handler, o => o.DefaultHeaders = new Dictionary<string, string> { ["Content-Encoding"] = "gzip" });

        await SendAsync(client, new string('a', 5000));

        Assert.True(client.Compression);
        Assert.Equal("zstd", Assert.Single(handler.Requests).Headers["Content-Encoding"]);
    }

    [Fact]
    public async Task Retries_resend_the_same_compressed_body()
    {
        var handler = ScriptedHandler.Sequence(
            ScriptedHandler.JsonResponse(new { }, HttpStatusCode.InternalServerError),
            ScriptedHandler.JsonResponse(Fixtures.SystemOne));
        using var client = TestClient.Create(handler, o => o.Retry = new RetryPolicy { MaxRetries = 1, BackoffInitialMs = 1, BackoffMaxMs = 1, BackoffJitter = 0 });

        await SendAsync(client, new string('a', 5000));

        Assert.Equal(2, handler.Requests.Count);
        Assert.All(handler.Requests, r => Assert.Equal("zstd", r.Headers["Content-Encoding"]));
        Assert.Equal(handler.Requests[0].BodyBytes, handler.Requests[1].BodyBytes);
    }
}
