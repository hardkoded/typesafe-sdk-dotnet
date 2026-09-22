// Copyright (c) Hardkoded.
// Licensed under the MIT License.

using System.Net;
using Xunit;

namespace TypeSafe.AI.Sdk.Tests;

public sealed class RetryAndErrorTests
{
    [Fact]
    public async Task Retries_a_429_then_succeeds_honoring_Retry_After()
    {
        var calls = 0;
        var handler = new ScriptedHandler((_, _) =>
        {
            calls++;
            return calls == 1
                ? ScriptedHandler.JsonResponse(new { }, HttpStatusCode.TooManyRequests, new Dictionary<string, string> { ["retry-after"] = "0" })
                : ScriptedHandler.JsonResponse(Fixtures.Models);
        });
        var logger = new RecordingLogger();
        using var client = TestClient.Create(handler, o =>
        {
            o.Logger = logger;
            o.LogLevel = TypeSafeLogLevel.Info;
        });
        var models = await client.Models.ListAsync();
        Assert.Equal("m", models[0].Name);
        Assert.Equal(2, handler.Requests.Count);
        Assert.Contains(logger.Lines, line => line.Contains("retrying in 0ms (retry 1/2) after 429"));
        Assert.False(handler.Requests[0].Headers.ContainsKey("X-TypeSafe-Retry-Count"));
        Assert.Equal("1", handler.Requests[1].Headers["X-TypeSafe-Retry-Count"]);
    }

    [Fact]
    public async Task Uses_exponential_backoff_when_there_is_no_Retry_After()
    {
        var calls = 0;
        var handler = new ScriptedHandler((_, _) =>
        {
            calls++;
            return calls <= 2
                ? ScriptedHandler.JsonResponse(new { }, HttpStatusCode.ServiceUnavailable)
                : ScriptedHandler.JsonResponse(Fixtures.Models);
        });
        using var client = TestClient.Create(handler, o =>
        {
            o.Retry = new RetryPolicy { BackoffJitter = 0 };
        });
        var models = await client.Models.ListAsync();
        Assert.Equal("m", models[0].Name);
        Assert.Equal(3, handler.Requests.Count);
        Assert.Equal("1", handler.Requests[1].Headers["X-TypeSafe-Retry-Count"]);
        Assert.Equal("2", handler.Requests[2].Headers["X-TypeSafe-Retry-Count"]);
    }

    [Fact]
    public async Task Gives_up_after_maxRetries_and_throws_the_last_error()
    {
        var handler = new ScriptedHandler((_, _) =>
            ScriptedHandler.JsonResponse(new { message = "down" }, HttpStatusCode.InternalServerError));
        using var client = TestClient.Create(handler, o => o.Retry = new RetryPolicy { MaxRetries = 3, BackoffJitter = 0, BackoffInitialMs = 0 });
        await Assert.ThrowsAsync<InternalServerException>(() => client.Models.ListAsync());
        Assert.Equal(4, handler.Requests.Count);
    }

    [Fact]
    public async Task Does_not_retry_non_retryable_statuses()
    {
        var handler = new ScriptedHandler((_, _) => ScriptedHandler.JsonResponse(new { }, HttpStatusCode.BadRequest));
        using var client = TestClient.Create(handler);
        await Assert.ThrowsAsync<BadRequestException>(() => client.Models.ListAsync());
        Assert.Single(handler.Requests);
    }

    [Fact]
    public async Task Retries_connection_errors()
    {
        var calls = 0;
        var handler = new ScriptedHandler((_, _) =>
        {
            calls++;
            if (calls == 1)
            {
                throw new HttpRequestException("fetch failed");
            }

            return ScriptedHandler.JsonResponse(Fixtures.Models);
        });
        using var client = TestClient.Create(handler, o => o.Retry = new RetryPolicy { BackoffInitialMs = 0, BackoffJitter = 0 });
        var models = await client.Models.ListAsync();
        Assert.Equal("m", models[0].Name);
        Assert.Equal(2, handler.Requests.Count);
    }

    [Fact]
    public async Task Never_retries_a_caller_abort()
    {
        using var cts = new CancellationTokenSource();
        var handler = new ScriptedHandler((_, _) =>
        {
            cts.Cancel();
            throw new OperationCanceledException(cts.Token);
        });
        using var client = TestClient.Create(handler);
        await Assert.ThrowsAsync<ApiUserAbortException>(() => client.Models.ListAsync(cancellationToken: cts.Token));
        Assert.Single(handler.Requests);
    }

    [Fact]
    public async Task Per_call_maxRetries_of_zero_disables_retries()
    {
        var handler = new ScriptedHandler((_, _) => ScriptedHandler.JsonResponse(new { }, HttpStatusCode.ServiceUnavailable));
        using var client = TestClient.Create(handler, o => o.Retry = new RetryPolicy { MaxRetries = 5 });
        await Assert.ThrowsAsync<InternalServerException>(() =>
            client.Models.ListAsync(new RequestOptions { Retry = new RetryPolicy { MaxRetries = 0 } }));
        Assert.Single(handler.Requests);
        Assert.Equal(5, client.Retry.MaxRetries);
    }

    [Fact]
    public void Rejects_invalid_maxRetries_from_config_or_per_call()
    {
        Assert.Throws<TypeSafeException>(() =>
            new TypeSafeClient(new TypeSafeClientOptions { ApiKey = "k", Retry = new RetryPolicy { MaxRetries = -1 } }));
        using var client = TestClient.Create(ScriptedHandler.Json(new { models = Array.Empty<object>() }));
        var ex = Assert.Throws<TypeSafeException>(() =>
            client.Models.ListAsync(new RequestOptions { Retry = new RetryPolicy { MaxRetries = -1 } }).GetAwaiter().GetResult());
        Assert.Contains("retry.maxRetries", ex.Message);
    }

    [Fact]
    public void Isolates_policy_and_status_set_between_clients()
    {
        using var first = new TypeSafeClient(new TypeSafeClientOptions { ApiKey = "k" });
        using var second = new TypeSafeClient(new TypeSafeClientOptions { ApiKey = "k" });
        Assert.NotSame(first.Retry, second.Retry);
        first.Retry.MaxRetries = 0;
        first.Retry.HttpStatuses.Clear();
        Assert.Equal(2, second.Retry.MaxRetries);
        Assert.True(second.Retry.HttpStatuses.Contains(503));
    }

    [Fact]
    public async Task Copies_caller_owned_status_sets()
    {
        var statuses = new HashSet<int> { 503 };
        var handler = new ScriptedHandler((_, _) => ScriptedHandler.JsonResponse(new { }, HttpStatusCode.ServiceUnavailable));
        using var client = TestClient.Create(handler, o => o.Retry = new RetryPolicy
        {
            HttpStatuses = statuses,
            MaxRetries = 1,
            BackoffInitialMs = 0,
        });
        statuses.Clear();
        await Assert.ThrowsAsync<InternalServerException>(() => client.Models.ListAsync());
        Assert.Equal(2, handler.Requests.Count);
    }

    [Fact]
    public void Validates_every_numeric_retry_field()
    {
        TypeSafeException Bad(Action<RetryPolicy> configure)
        {
            var retry = new RetryPolicy();
            configure(retry);
            return Assert.Throws<TypeSafeException>(() =>
                new TypeSafeClient(new TypeSafeClientOptions { ApiKey = "k", Retry = retry }));
        }

        Assert.Contains("retry.backoffInitialMs", Bad(r => r.BackoffInitialMs = -1).Message);
        Assert.Contains("retry.backoffJitter", Bad(r => r.BackoffJitter = 1.5).Message);
        Assert.Contains("retry.backoffJitter", Bad(r => r.BackoffJitter = -0.1).Message);
        Assert.Contains("retry.httpStatuses", Bad(r => r.HttpStatuses = new HashSet<int> { 503, 42 }).Message);
        Assert.Null(Record.Exception(() =>
            new TypeSafeClient(new TypeSafeClientOptions
            {
                ApiKey = "k",
                Retry = new RetryPolicy { BackoffInitialMs = 0, BackoffMaxMs = 0, BackoffJitter = 0 },
            })));
    }

    [Fact]
    public async Task Can_stop_retrying_connection_errors()
    {
        var handler = new ScriptedHandler((_, _) => throw new HttpRequestException("fetch failed"));
        using var client = TestClient.Create(handler, o => o.Retry = new RetryPolicy { ApiConnectionError = false });
        var ex = await Assert.ThrowsAsync<ApiConnectionException>(() => client.Models.ListAsync());
        Assert.Contains("fetch failed", ex.Message);
        Assert.Single(handler.Requests);
    }

    [Fact]
    public async Task Timeout_throws_ApiTimeoutException()
    {
        using var client = TestClient.Create(ScriptedHandler.Hang(), o =>
        {
            o.TimeoutMs = 50;
            o.Retry = new RetryPolicy { MaxRetries = 0 };
        });
        var ex = await Assert.ThrowsAsync<ApiTimeoutException>(() => client.Models.ListAsync());
        Assert.IsAssignableFrom<ApiConnectionException>(ex);
        Assert.Equal(50, ex.TimeoutMs);
        Assert.Equal("Request timed out after 50ms.", ex.Message);
    }

    [Fact]
    public async Task Caller_abort_during_hung_request_is_abort_not_timeout()
    {
        using var cts = new CancellationTokenSource();
        using var client = TestClient.Create(ScriptedHandler.Hang(), o => o.TimeoutMs = 60_000);
        var pending = client.Models.ListAsync(cancellationToken: cts.Token);
        await Task.Delay(30);
        cts.Cancel();
        await Assert.ThrowsAsync<ApiUserAbortException>(() => pending);
    }

    [Fact]
    public void Rejects_invalid_timeouts()
    {
        Assert.Throws<TypeSafeException>(() =>
            new TypeSafeClient(new TypeSafeClientOptions { ApiKey = "k", TimeoutMs = 0 }));
        using var client = TestClient.Create(ScriptedHandler.Json(new { models = Array.Empty<object>() }));
        var ex = Assert.Throws<TypeSafeException>(() =>
            client.Models.ListAsync(new RequestOptions { TimeoutMs = -5 }).GetAwaiter().GetResult());
        Assert.Contains("timeout", ex.Message);
    }

    [Theory]
    [InlineData(400, typeof(BadRequestException))]
    [InlineData(401, typeof(AuthenticationException))]
    [InlineData(403, typeof(PermissionDeniedException))]
    [InlineData(404, typeof(NotFoundException))]
    [InlineData(422, typeof(UnprocessableEntityException))]
    [InlineData(429, typeof(RateLimitException))]
    [InlineData(500, typeof(InternalServerException))]
    [InlineData(503, typeof(InternalServerException))]
    [InlineData(418, typeof(ApiException))]
    public async Task Maps_status_to_exception_type(int status, Type expected)
    {
        var handler = new ScriptedHandler((_, _) =>
            ScriptedHandler.JsonResponse(new { }, (HttpStatusCode)status));
        using var client = TestClient.Create(handler, o => o.Retry = new RetryPolicy { MaxRetries = 0 });
        var ex = await Assert.ThrowsAnyAsync<ApiException>(() => client.Models.ListAsync());
        Assert.IsType(expected, ex);
        Assert.Equal(status, ex.Status);
    }

    [Fact]
    public async Task Uses_error_message_from_json_body_and_exposes_request_id()
    {
        var handler = new ScriptedHandler((_, _) => ScriptedHandler.JsonResponse(
            new { error = new { message = "invalid api key" } },
            HttpStatusCode.Unauthorized,
            new Dictionary<string, string> { ["x-typesafe-request-id"] = "req_123" }));
        using var client = TestClient.Create(handler, o => o.Retry = new RetryPolicy { MaxRetries = 0 });
        var ex = await Assert.ThrowsAsync<AuthenticationException>(() => client.Models.ListAsync());
        Assert.Equal("401 invalid api key", ex.Message);
        Assert.Equal("req_123", ex.RequestId);
    }

    [Theory]
    [InlineData("{\"error\":\"plain string\"}", "plain string")]
    [InlineData("{\"message\":\"top-level message\"}", "top-level message")]
    [InlineData("{\"detail\":\"fastapi style\"}", "fastapi style")]
    [InlineData("{\"detail\":{\"error_type\":\"api_usage_error\",\"message\":\"Unknown model: x\"}}", "Unknown model: x")]
    public async Task Extracts_a_message_from_common_error_shapes(string body, string expected)
    {
        var handler = new ScriptedHandler((_, _) => ScriptedHandler.JsonResponse(body, HttpStatusCode.BadRequest));
        using var client = TestClient.Create(handler, o => o.Retry = new RetryPolicy { MaxRetries = 0 });
        var ex = await Assert.ThrowsAsync<BadRequestException>(() => client.Models.ListAsync());
        Assert.Equal($"400 {expected}", ex.Message);
    }

    [Fact]
    public async Task Formats_validation_errors()
    {
        var body = new
        {
            detail = new object[]
            {
                new { type = "list_type", loc = new object[] { "body", "questions", "q", "score", "criteria" }, msg = "Input should be a valid list" },
                new { type = "too_short", loc = new object[] { "body", "questions" }, msg = "Dictionary should have at least 1 item" },
            },
        };
        var handler = new ScriptedHandler((_, _) => ScriptedHandler.JsonResponse(body, HttpStatusCode.BadRequest));
        using var client = TestClient.Create(handler, o => o.Retry = new RetryPolicy { MaxRetries = 0 });
        var ex = await Assert.ThrowsAsync<BadRequestException>(() => client.Models.ListAsync());
        Assert.Equal("400 questions.q.score.criteria: Input should be a valid list; questions: Dictionary should have at least 1 item", ex.Message);
    }

    [Fact]
    public async Task Falls_back_to_raw_body_truncated()
    {
        var handler = new ScriptedHandler((_, _) => ScriptedHandler.JsonResponse(new { code = 7 }, HttpStatusCode.BadRequest));
        using var client = TestClient.Create(handler, o => o.Retry = new RetryPolicy { MaxRetries = 0 });
        var shortErr = await Assert.ThrowsAsync<BadRequestException>(() => client.Models.ListAsync());
        Assert.Equal("400 {\"code\":7}", shortErr.Message);

        var longHandler = new ScriptedHandler((_, _) =>
            ScriptedHandler.JsonResponse(new { blob = new string('x', 500) }, HttpStatusCode.BadRequest));
        using var longClient = TestClient.Create(longHandler, o => o.Retry = new RetryPolicy { MaxRetries = 0 });
        var longErr = await Assert.ThrowsAsync<BadRequestException>(() => longClient.Models.ListAsync());
        Assert.Equal("400 ".Length + 200 + 1, longErr.Message.Length);
        Assert.EndsWith("…", longErr.Message);
    }

    [Fact]
    public async Task Keeps_non_json_bodies_as_text()
    {
        var handler = new ScriptedHandler((_, _) => new HttpResponseMessage(HttpStatusCode.BadGateway)
        {
            Content = new StringContent(" bad gateway ", System.Text.Encoding.UTF8, "text/html"),
        });
        using var client = TestClient.Create(handler, o => o.Retry = new RetryPolicy { MaxRetries = 0 });
        var ex = await Assert.ThrowsAsync<InternalServerException>(() => client.Models.ListAsync());
        Assert.Equal(" bad gateway ", ex.Body);
        Assert.Equal("502  bad gateway ", ex.Message);
    }

    [Fact]
    public async Task Handles_empty_bodies()
    {
        var handler = new ScriptedHandler((_, _) => ScriptedHandler.Empty(HttpStatusCode.TooManyRequests));
        using var client = TestClient.Create(handler, o => o.Retry = new RetryPolicy { MaxRetries = 0 });
        var ex = await Assert.ThrowsAsync<RateLimitException>(() => client.Models.ListAsync());
        Assert.Null(ex.Body);
        Assert.Equal("429 status code (no body)", ex.Message);
        Assert.Null(ex.RetryAfterMs);
    }

    [Fact]
    public async Task Rate_limit_exposes_parsed_retry_after()
    {
        var handler = new ScriptedHandler((_, _) =>
            ScriptedHandler.Empty(HttpStatusCode.TooManyRequests, new Dictionary<string, string> { ["retry-after"] = "7" }));
        using var client = TestClient.Create(handler, o => o.Retry = new RetryPolicy { MaxRetries = 0 });
        var ex = await Assert.ThrowsAsync<RateLimitException>(() => client.Models.ListAsync());
        Assert.Equal(7000, ex.RetryAfterMs);
    }

    [Fact]
    public void Retry_delay_grows_exponentially_and_honors_headers()
    {
        var policy = RetryPolicy.Default;
        int Delay(int attempt, Func<double> random) => RetryCalculator.DelayMs(attempt, null, policy, random);
        Assert.Equal(new[] { 500, 1000, 2000, 4000, 5000, 5000 }, new[] { 0, 1, 2, 3, 4, 5 }.Select(a => Delay(a, () => 0)).ToArray());
        Assert.Equal(375, Delay(0, () => 1));
        Assert.Equal(875, Delay(1, () => 0.5));

        using var response = ScriptedHandler.Empty(HttpStatusCode.TooManyRequests, new Dictionary<string, string> { ["retry-after"] = "2" });
        Assert.Equal(2000, RetryCalculator.DelayMs(0, response.Headers, policy, () => 1));

        using var tooLong = ScriptedHandler.Empty(HttpStatusCode.TooManyRequests, new Dictionary<string, string> { ["retry-after"] = "61" });
        Assert.Equal(500, RetryCalculator.DelayMs(0, tooLong.Headers, policy, () => 0));

        using var exactCap = ScriptedHandler.Empty(HttpStatusCode.TooManyRequests, new Dictionary<string, string> { ["retry-after"] = "60" });
        Assert.Equal(60_000, RetryCalculator.DelayMs(0, exactCap.Headers, policy, () => 0));
    }

    [Fact]
    public void ParseRetryAfter_prefers_ms_and_reads_http_dates()
    {
        using var both = ScriptedHandler.Empty(HttpStatusCode.OK, new Dictionary<string, string>
        {
            ["retry-after-ms"] = "250",
            ["retry-after"] = "3",
        });
        Assert.Equal(250, RetryCalculator.ParseRetryAfter(both.Headers));

        var now = DateTimeOffset.Parse("Wed, 21 Oct 2026 07:28:00 GMT");
        using var date = ScriptedHandler.Empty(HttpStatusCode.OK, new Dictionary<string, string>
        {
            ["retry-after"] = "Wed, 21 Oct 2026 07:28:05 GMT",
        });
        Assert.Equal(5000, RetryCalculator.ParseRetryAfter(date.Headers, now));
    }
}

