// Copyright (c) Hardkoded.
// Licensed under the MIT License.

using System.Globalization;
using System.Net.Http.Headers;

namespace TypeSafe.AI.Sdk;

/// <summary>Retry delay calculation and cancellable waits. Mirrors JS <c>retry.ts</c>.</summary>
public static class RetryCalculator
{
    /// <summary>Default timeout per attempt in milliseconds.</summary>
    public const int DefaultTimeoutMs = 10_000;

    /// <summary>Whether the policy retries an HTTP status code.</summary>
    public static bool IsRetryableStatus(int status, RetryPolicy? policy = null) =>
        (policy ?? RetryPolicy.Default).HttpStatuses.Contains(status);

    /// <summary>
    /// Parse <c>retry-after-ms</c> or <c>Retry-After</c> into milliseconds, preferring <c>retry-after-ms</c>.
    /// Returns <c>null</c> when neither header contains a valid delay.
    /// </summary>
    public static int? ParseRetryAfter(HttpResponseHeaders headers, DateTimeOffset? now = null)
    {
        if (TryGetHeader(headers, "retry-after-ms", out var retryAfterMsRaw)
            && double.TryParse(retryAfterMsRaw, NumberStyles.Float, CultureInfo.InvariantCulture, out var ms)
            && ms >= 0
            && !double.IsNaN(ms)
            && !double.IsInfinity(ms))
        {
            return (int)ms;
        }

        if (!TryGetHeader(headers, "Retry-After", out var raw) && !TryGetHeader(headers, "retry-after", out raw))
        {
            return null;
        }

        if (double.TryParse(raw, NumberStyles.Float, CultureInfo.InvariantCulture, out var seconds)
            && !double.IsNaN(seconds)
            && !double.IsInfinity(seconds))
        {
            return seconds >= 0 ? (int)(seconds * 1000) : null;
        }

        if (DateTimeOffset.TryParse(raw, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var date))
        {
            var delay = date - (now ?? DateTimeOffset.UtcNow);
            return (int)Math.Max(0, delay.TotalMilliseconds);
        }

        return null;
    }

    /// <summary>
    /// Calculate the delay in milliseconds for a zero-based retry attempt.
    /// Uses an allowed server delay; otherwise uses capped exponential backoff with jitter.
    /// </summary>
    public static int DelayMs(
        int attempt,
        HttpResponseHeaders? headers = null,
        RetryPolicy? policy = null,
        Func<double>? random = null)
    {
        policy ??= RetryPolicy.Default;
        if (policy.RespectRetryAfter && headers is not null)
        {
            var retryAfter = ParseRetryAfter(headers);
            if (retryAfter is int serverDelay && serverDelay <= policy.MaxRetryAfterMs)
            {
                return serverDelay;
            }
        }

        var exponential = Math.Min(policy.BackoffInitialMs * Math.Pow(2, attempt), policy.BackoffMaxMs);
        var sample = random?.Invoke() ?? NextDouble();
        return (int)Math.Round(exponential * (1 - sample * policy.BackoffJitter));
    }

    /// <summary>Wait <paramref name="milliseconds"/>, throwing <see cref="OperationCanceledException"/> on cancellation.</summary>
    public static Task SleepAsync(int milliseconds, CancellationToken cancellationToken = default) =>
        Task.Delay(milliseconds, cancellationToken);

#if !NET6_0_OR_GREATER
    private static readonly object s_randomLock = new();
    private static readonly Random s_randomInstance = new();
#endif

    private static double NextDouble()
    {
#if NET6_0_OR_GREATER
        return Random.Shared.NextDouble();
#else
        lock (s_randomLock)
        {
            return s_randomInstance.NextDouble();
        }
#endif
    }

    private static bool TryGetHeader(HttpResponseHeaders headers, string name, out string value)
    {
        if (headers.TryGetValues(name, out var values))
        {
            value = values.FirstOrDefault() ?? "";
            return !string.IsNullOrEmpty(value);
        }

        value = "";
        return false;
    }
}
