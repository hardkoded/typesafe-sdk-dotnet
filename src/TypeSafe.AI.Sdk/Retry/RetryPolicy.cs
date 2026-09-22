// Copyright (c) Dario Kondratiuk.
// Licensed under the MIT License.

namespace TypeSafe.AI.Sdk;

/// <summary>
/// Retry configuration. Partial overrides inherit unset fields from the client or SDK defaults.
/// Defaults match the JavaScript SDK.
/// </summary>
public sealed class RetryPolicy
{
    private int _maxRetries = 2;
    private int _backoffInitialMs = 500;
    private int _backoffMaxMs = 5000;
    private double _backoffJitter = 0.25;
    private ISet<int> _httpStatuses = DefaultHttpStatuses();
    private bool _respectRetryAfter = true;
    private int _maxRetryAfterMs = 60_000;
    private bool _apiConnectionError = true;
    private bool _apiTimeoutError = true;

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
        get => _maxRetries;
        set
        {
            _maxRetries = value;
            MaxRetriesSpecified = true;
        }
    }

    /// <summary>First backoff delay in milliseconds, doubled up to <see cref="BackoffMaxMs"/>. Default: 500.</summary>
    public int BackoffInitialMs
    {
        get => _backoffInitialMs;
        set
        {
            _backoffInitialMs = value;
            BackoffInitialMsSpecified = true;
        }
    }

    /// <summary>Maximum backoff delay in milliseconds. Default: 5000.</summary>
    public int BackoffMaxMs
    {
        get => _backoffMaxMs;
        set
        {
            _backoffMaxMs = value;
            BackoffMaxMsSpecified = true;
        }
    }

    /// <summary>Fraction of each backoff delay randomly subtracted, from 0 to 1. Default: 0.25.</summary>
    public double BackoffJitter
    {
        get => _backoffJitter;
        set
        {
            _backoffJitter = value;
            BackoffJitterSpecified = true;
        }
    }

    /// <summary>HTTP status codes to retry. Default: 408, 429, and 500–599.</summary>
    public ISet<int> HttpStatuses
    {
        get => _httpStatuses;
        set
        {
            _httpStatuses = value ?? throw new TypeSafeException("`retry.httpStatuses` must contain HTTP status codes, got null.");
            HttpStatusesSpecified = true;
        }
    }

    /// <summary>Honor <c>Retry-After</c> and <c>retry-after-ms</c> up to <see cref="MaxRetryAfterMs"/>. Default: true.</summary>
    public bool RespectRetryAfter
    {
        get => _respectRetryAfter;
        set
        {
            _respectRetryAfter = value;
            RespectRetryAfterSpecified = true;
        }
    }

    /// <summary>Maximum server retry delay in milliseconds; longer delays use backoff. Default: 60000.</summary>
    public int MaxRetryAfterMs
    {
        get => _maxRetryAfterMs;
        set
        {
            _maxRetryAfterMs = value;
            MaxRetryAfterMsSpecified = true;
        }
    }

    /// <summary>Retry connection failures, including interrupted response bodies. Default: true.</summary>
    public bool ApiConnectionError
    {
        get => _apiConnectionError;
        set
        {
            _apiConnectionError = value;
            ApiConnectionErrorSpecified = true;
        }
    }

    /// <summary>Whether to retry <see cref="ApiTimeoutException"/>. Default: true.</summary>
    public bool ApiTimeoutError
    {
        get => _apiTimeoutError;
        set
        {
            _apiTimeoutError = value;
            ApiTimeoutErrorSpecified = true;
        }
    }

    /// <summary>SDK default retry policy (a new isolated copy).</summary>
    public static RetryPolicy Default => new();

    /// <summary>Copy this policy so later mutations cannot affect in-flight requests.</summary>
    public RetryPolicy Clone()
    {
        var clone = new RetryPolicy();
        clone._maxRetries = _maxRetries;
        clone._backoffInitialMs = _backoffInitialMs;
        clone._backoffMaxMs = _backoffMaxMs;
        clone._backoffJitter = _backoffJitter;
        clone._httpStatuses = new HashSet<int>(_httpStatuses);
        clone._respectRetryAfter = _respectRetryAfter;
        clone._maxRetryAfterMs = _maxRetryAfterMs;
        clone._apiConnectionError = _apiConnectionError;
        clone._apiTimeoutError = _apiTimeoutError;
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
            merged._maxRetries = Guards.NonNegativeInteger("retry.maxRetries", overrides.MaxRetries);
        }

        if (overrides.BackoffInitialMsSpecified)
        {
            merged._backoffInitialMs = Guards.NonNegativeMs("retry.backoffInitialMs", overrides.BackoffInitialMs);
        }

        if (overrides.BackoffMaxMsSpecified)
        {
            merged._backoffMaxMs = Guards.NonNegativeMs("retry.backoffMaxMs", overrides.BackoffMaxMs);
        }

        if (overrides.BackoffJitterSpecified)
        {
            merged._backoffJitter = Guards.Fraction("retry.backoffJitter", overrides.BackoffJitter);
        }

        if (overrides.HttpStatusesSpecified)
        {
            merged._httpStatuses = new HashSet<int>(Guards.StatusSet("retry.httpStatuses", overrides.HttpStatuses));
        }

        if (overrides.RespectRetryAfterSpecified)
        {
            merged._respectRetryAfter = overrides.RespectRetryAfter;
        }

        if (overrides.MaxRetryAfterMsSpecified)
        {
            merged._maxRetryAfterMs = Guards.NonNegativeMs("retry.maxRetryAfterMs", overrides.MaxRetryAfterMs);
        }

        if (overrides.ApiConnectionErrorSpecified)
        {
            merged._apiConnectionError = overrides.ApiConnectionError;
        }

        if (overrides.ApiTimeoutErrorSpecified)
        {
            merged._apiTimeoutError = overrides.ApiTimeoutError;
        }

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
        _httpStatuses = new HashSet<int>(_httpStatuses);
        return this;
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
