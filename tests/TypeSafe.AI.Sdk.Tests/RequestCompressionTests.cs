// Copyright (c) Dario Kondratiuk.
// Licensed under the MIT License.

using System.Net;
using System.Text;
using Xunit;

namespace TypeSafe.AI.Sdk.Tests;

public sealed class RequestCompressionTests
{
    private static readonly string s_largeState = new('x', RequestCompressor.MinCompressedBodyBytes);

    [Fact]
    public void Default_and_explicit_zstd_resolve_to_zstd_when_available()
    {
        using var auto = TestClient.Create(ScriptedHandler.Json(Fixtures.SystemOne));
        Assert.Equal(RequestCompression.Zstd, auto.Compression);

        using var explicitZstd = TestClient.Create(ScriptedHandler.Json(Fixtures.SystemOne), o =>
        {
            o.Compression = RequestCompression.Zstd;
        });
        Assert.Equal(RequestCompression.Zstd, explicitZstd.Compression);
    }

    [Fact]
    public async Task Bodies_under_1_kib_are_sent_uncompressed()
    {
        var (handler, client) = await SendAsync(state: "x");
        using (client)
        {
            Assert.Equal(RequestCompression.Zstd, client.Compression);
            Assert.False(handler.Requests[0].Headers.ContainsKey("Content-Encoding"));
            Assert.Equal("x", handler.Requests[0].Body!.Value.GetProperty("state").GetString());
        }
    }

    [Fact]
    public async Task Bodies_of_1_kib_or_more_are_sent_as_zstd()
    {
        var (handler, client) = await SendAsync(s_largeState);
        using (client)
        {
            Assert.Equal("zstd", handler.Requests[0].Headers["Content-Encoding"]);
            Assert.StartsWith("application/json", handler.Requests[0].Headers["Content-Type"]);
            var json = Encoding.UTF8.GetString(RequestCompressor.Decompress(handler.Requests[0].BodyBytes!));
            Assert.Contains(s_largeState, json);
            Assert.Equal(s_largeState, System.Text.Json.JsonDocument.Parse(json).RootElement.GetProperty("state").GetString());
        }
    }

    [Fact]
    public async Task Measures_the_body_in_utf8_bytes_not_char_length()
    {
        // Each "é" is one UTF-16 code unit and two UTF-8 bytes.
        var accented = new string('é', RequestCompressor.MinCompressedBodyBytes / 2);
        Assert.Equal(RequestCompressor.MinCompressedBodyBytes / 2, accented.Length);
        Assert.Equal(RequestCompressor.MinCompressedBodyBytes, Encoding.UTF8.GetByteCount(accented));

        var (handler, client) = await SendAsync(accented);
        using (client)
        {
            Assert.Equal("zstd", handler.Requests[0].Headers["Content-Encoding"]);
            var json = Encoding.UTF8.GetString(RequestCompressor.Decompress(handler.Requests[0].BodyBytes!));
            Assert.Equal(accented, System.Text.Json.JsonDocument.Parse(json).RootElement.GetProperty("state").GetString());
        }
    }

    [Fact]
    public async Task Compression_none_never_compresses_and_drops_caller_content_encoding()
    {
        var (handler, client) = await SendAsync(
            s_largeState,
            o => o.Compression = RequestCompression.None,
            new Dictionary<string, string> { ["content-encoding"] = "br" });
        using (client)
        {
            Assert.Equal(RequestCompression.None, client.Compression);
            Assert.False(handler.Requests[0].Headers.ContainsKey("Content-Encoding"));
            Assert.Equal(s_largeState, handler.Requests[0].Body!.Value.GetProperty("state").GetString());
        }
    }

    [Fact]
    public async Task Replaces_a_caller_content_encoding_with_zstd()
    {
        var (handler, client) = await SendAsync(
            s_largeState,
            headers: new Dictionary<string, string> { ["Content-Encoding"] = "br" });
        using (client)
        {
            Assert.Equal("zstd", handler.Requests[0].Headers["Content-Encoding"]);
        }
    }

    [Fact]
    public async Task Get_requests_omit_content_encoding()
    {
        var handler = ScriptedHandler.Json(Fixtures.Models);
        using var client = TestClient.Create(handler);
        await client.Models.ListAsync();
        Assert.False(handler.Requests[0].Headers.ContainsKey("Content-Encoding"));
        Assert.Null(handler.Requests[0].BodyBytes);
    }

    [Fact]
    public async Task Compresses_once_and_reuses_the_bytes_on_retries()
    {
        var calls = 0;
        var handler = new ScriptedHandler((_, _) =>
        {
            calls++;
            return calls == 1
                ? ScriptedHandler.JsonResponse(
                    new { },
                    HttpStatusCode.TooManyRequests,
                    new Dictionary<string, string> { ["retry-after"] = "0" })
                : ScriptedHandler.JsonResponse(Fixtures.SystemOne);
        });
        using var client = TestClient.Create(handler);
        await client.SystemOneAsync(s_largeState, Question.Map(("q1", Question.Noul("?"))));
        Assert.Equal(2, handler.Requests.Count);
        Assert.Equal(handler.Requests[0].BodyBytes, handler.Requests[1].BodyBytes);
        Assert.Equal("zstd", handler.Requests[0].Headers["Content-Encoding"]);
        Assert.Equal("zstd", handler.Requests[1].Headers["Content-Encoding"]);
    }

    [Fact]
    public async Task Debug_logs_show_the_uncompressed_payload()
    {
        var handler = ScriptedHandler.Json(Fixtures.SystemOne);
        var logger = new RecordingLogger();
        using var client = TestClient.Create(handler, o =>
        {
            o.Logger = logger;
            o.LogLevel = TypeSafeLogLevel.Debug;
        });
        await client.SystemOneAsync(s_largeState, Question.Map(("q1", Question.Noul("?"))));
        var outgoing = logger.Entries.First(e => e.Message.Contains("->", StringComparison.Ordinal));
        var payload = outgoing.Args[0]!;
        var payloadType = payload.GetType();
        var body = payloadType.GetProperty("body")!.GetValue(payload);
        var headers = (IReadOnlyDictionary<string, string>)payloadType.GetProperty("headers")!.GetValue(payload)!;
        Assert.Contains(s_largeState, body!.ToString());
        Assert.Equal("zstd", headers["Content-Encoding"]);
    }

    [Fact]
    public async Task Sends_uncompressed_without_zstd_and_rejects_an_explicit_zstd()
    {
        using var unavailable = RequestCompressor.OverrideAvailability(false);
        var (handler, client) = await SendAsync(s_largeState);
        using (client)
        {
            Assert.Equal(RequestCompression.None, client.Compression);
            Assert.False(handler.Requests[0].Headers.ContainsKey("Content-Encoding"));
            Assert.Equal(s_largeState, handler.Requests[0].Body!.Value.GetProperty("state").GetString());
        }

        var ex = Assert.Throws<TypeSafeException>(() =>
            new TypeSafeClient(new TypeSafeClientOptions { ApiKey = "k", Compression = RequestCompression.Zstd }));
        Assert.Contains("zstd compression is not available in this runtime", ex.Message);
        Assert.Contains("RequestCompression.None", ex.Message);
    }

    [Theory]
    [InlineData(3)]
    [InlineData(99)]
    public void Rejects_invalid_compression_values(int value)
    {
        var ex = Assert.Throws<TypeSafeException>(() =>
            new TypeSafeClient(new TypeSafeClientOptions { ApiKey = "k", Compression = (RequestCompression)value }));
        Assert.Contains("`compression` must be Auto, Zstd, or None", ex.Message);
        Assert.Contains(value.ToString(), ex.Message);
    }

    [Fact]
    public void Byte_length_counts_utf8()
    {
        Assert.Equal(1, RequestCompressor.ByteLength("x"));
        Assert.Equal(2, RequestCompressor.ByteLength("é"));
        Assert.Equal(RequestCompressor.MinCompressedBodyBytes, RequestCompressor.ByteLength(s_largeState));
    }

    private static async Task<(ScriptedHandler Handler, TypeSafeClient Client)> SendAsync(
        string state,
        Action<TypeSafeClientOptions>? configure = null,
        IDictionary<string, string>? headers = null)
    {
        var handler = ScriptedHandler.Json(Fixtures.SystemOne);
        var client = TestClient.Create(handler, configure);
        await client.SystemOneAsync(
            state,
            Question.Map(("q1", Question.Noul("?"))),
            options: headers is null ? null : new RequestOptions { Headers = headers });
        return (handler, client);
    }
}
