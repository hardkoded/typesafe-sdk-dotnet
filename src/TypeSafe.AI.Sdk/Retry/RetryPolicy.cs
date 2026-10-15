// Copyright (c) Dario Kondratiuk.
// Licensed under the MIT License.

namespace TypeSafe.AI.Sdk;

/// <summary>
/// Retry configuration. Partial overrides inherit unset fields from the client or SDK defaults.
/// Defaults match the JavaScript SDK.
/// </summary>
public sealed class RetryPolicy
{
    internal bool MaxRetriesSpecified { get; private set; }
    internal bool BackoffInitialMsSpecified { get; private set; }
    internal bool BackoffMaxMsSpecified { get; private set; }
    internal bool BackoffJitterSpecified { get; private set; }
    internal bool HttpStatusesSpecified { get; private set; }
    internal bool RespectRetryAfterSpecified { get; private set; }
    internal bool MaxRetryAfterMsSpecified { get; private set; }
    internal bool ApiConnectionErrorSpecified { get; private set; }
    internal bool ApiTimeoutErrorSpecified { get; private set; }

    /// <summary>Maximum retries after the initial attempt; <c>0</c> disables retries. Default: 2.</summary>
    public int MaxRetries
    {
        get;
        set
        {
            field = value;
            MaxRetriesSpecified = true;
        }
    } = 2;

    /// <summary>First backoff delay in milliseconds, doubled up to <see cref="BackoffMaxMs"/>. Default: 500.</summary>
    public int BackoffInitialMs
    {
        get;
        set
        {
            field = value;
            BackoffInitialMsSpecified = true;
        }
    } = 500;

    /// <summary>Maximum backoff delay in milliseconds. Default: 5000.</summary>
    public int BackoffMaxMs
    {
        get;
        set
        {
            field = value;
            BackoffMaxMsSpecified = true;
        }
    } = 5000;

    /// <summary>Fraction of each backoff delay randomly subtracted, from 0 to 1. Default: 0.25.</summary>
    public double BackoffJitter
    {
        get;
        set
        {
            field = value;
            BackoffJitterSpecified = true;
        }
    } = 0.25;

    /// <summary>HTTP status codes to retry. Default: 408, 429, and 500–599.</summary>
    public ISet<int> HttpStatuses
    {
        get;
        set
        {
            field = value ?? throw new TypeSafeException("`retry.httpStatuses` must contain HTTP status codes, got null.");
            HttpStatusesSpecified = true;
        }
    } = DefaultHttpStatuses();

    /// <summary>Honor <c>Retry-After</c> and <c>retry-after-ms</c> up to <see cref="MaxRetryAfterMs"/>. Default: true.</summary>
    public bool RespectRetryAfter
    {
        get;
        set
        {
            field = value;
            RespectRetryAfterSpecified = true;
        }
    } = true;

    /// <summary>Maximum server retry delay in milliseconds; longer delays use backoff. Default: 60000.</summary>
    public int MaxRetryAfterMs
    {
        get;
        set
        {
            field = value;
            MaxRetryAfterMsSpecified = true;
        }
    } = 60_000;

    /// <summary>Retry connection failures, including interrupted response bodies. Default: true.</summary>
    public bool ApiConnectionError
    {
        get;
        set
        {
            field = value;
            ApiConnectionErrorSpecified = true;
        }
    } = true;

    /// <summary>Whether to retry <see cref="ApiTimeoutException"/>. Default: true.</summary>
    public bool ApiTimeoutError
    {
        get;
        set
        {
            field = value;
            ApiTimeoutErrorSpecified = true;
        }
    } = true;

    /// <summary>SDK default retry policy (a new isolated copy).</summary>
    public static RetryPolicy Default => new();

    /// <summary>Copy this policy so later mutations cannot affect in-flight requests.</summary>
    public RetryPolicy Clone()
    {
        var clone = new RetryPolicy();
        clone.MaxRetries = MaxRetries;
        clone.BackoffInitialMs = BackoffInitialMs;
        clone.BackoffMaxMs = BackoffMaxMs;
        clone.BackoffJitter = BackoffJitter;
        clone.HttpStatuses = new HashSet<int>(HttpStatuses);
        clone.RespectRetryAfter = RespectRetryAfter;
        clone.MaxRetryAfterMs = MaxRetryAfterMs;
        clone.ApiConnectionError = ApiConnectionError;
        clone.ApiTimeoutError = ApiTimeoutError;
        clone.ClearSpecified();
        return clone;
    }

    /// <summary>Merge overrides onto a base policy and validate every numeric field.</summary>
    public static RetryPolicy Resolve(RetryPolicy baseline, RetryPolicy? overrides)
    {
        var merged = baseline.Clone();
        if (overrides is null)
        {
            return merged;
        }

        if (overrides.MaxRetriesSpecified)
        {
            merged.MaxRetries = Guards.NonNegativeInteger("retry.maxRetries", overrides.MaxRetries);
        }

        if (overrides.BackoffInitialMsSpecified)
        {
            merged.BackoffInitialMs = Guards.NonNegativeMs("retry.backoffInitialMs", overrides.BackoffInitialMs);
        }

        if (overrides.BackoffMaxMsSpecified)
        {
            merged.BackoffMaxMs = Guards.NonNegativeMs("retry.backoffMaxMs", overrides.BackoffMaxMs);
        }

        if (overrides.BackoffJitterSpecified)
        {
            merged.BackoffJitter = Guards.Fraction("retry.backoffJitter", overrides.BackoffJitter);
        }

        if (overrides.HttpStatusesSpecified)
        {
            merged.HttpStatuses = new HashSet<int>(Guards.StatusSet("retry.httpStatuses", overrides.HttpStatuses));
        }

        if (overrides.RespectRetryAfterSpecified)
        {
            merged.RespectRetryAfter = overrides.RespectRetryAfter;
        }

        if (overrides.MaxRetryAfterMsSpecified)
        {
            merged.MaxRetryAfterMs = Guards.NonNegativeMs("retry.maxRetryAfterMs", overrides.MaxRetryAfterMs);
        }

        if (overrides.ApiConnectionErrorSpecified)
        {
            merged.ApiConnectionError = overrides.ApiConnectionError;
        }

        if (overrides.ApiTimeoutErrorSpecified)
        {
            merged.ApiTimeoutError = overrides.ApiTimeoutError;
        }

        merged.ClearSpecified();
        return merged;
    }

    /// <summary>Validate a fully-specified constructor policy.</summary>
    public RetryPolicy Validate()
    {
        Guards.NonNegativeInteger("retry.maxRetries", MaxRetries);
        Guards.NonNegativeMs("retry.backoffInitialMs", BackoffInitialMs);
        Guards.NonNegativeMs("retry.backoffMaxMs", BackoffMaxMs);
        Guards.Fraction("retry.backoffJitter", BackoffJitter);
        Guards.StatusSet("retry.httpStatuses", HttpStatuses);
        Guards.NonNegativeMs("retry.maxRetryAfterMs", MaxRetryAfterMs);
        var httpStatusesSpecified = HttpStatusesSpecified;
        HttpStatuses = new HashSet<int>(HttpStatuses);
        HttpStatusesSpecified = httpStatusesSpecified;
        return this;
    }

    private void ClearSpecified()
    {
        MaxRetriesSpecified = false;
        BackoffInitialMsSpecified = false;
        BackoffMaxMsSpecified = false;
        BackoffJitterSpecified = false;
        HttpStatusesSpecified = false;
        RespectRetryAfterSpecified = false;
        MaxRetryAfterMsSpecified = false;
        ApiConnectionErrorSpecified = false;
        ApiTimeoutErrorSpecified = false;
    }

    internal static ISet<int> DefaultHttpStatuses()
    {
        var set = new HashSet<int> { 408, 429 };
        for (var status = 500; status < 600; status++)
        {
            set.Add(status);
        }

        return set;
    }
}
