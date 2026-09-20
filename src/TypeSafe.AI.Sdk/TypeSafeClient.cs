using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace TypeSafe.AI.Sdk;

/// <summary>
/// Client for the TypeSafe AI API. Community .NET port of <c>@typesafe-ai/sdk</c>.
/// </summary>
public sealed class TypeSafeClient : ITypeSafeClient, IDisposable
{
    /// <summary>Named <see cref="HttpClient"/> used with <c>IHttpClientFactory</c>.</summary>
    public const string HttpClientName = "TypeSafe.AI";

    /// <summary>API root used when neither config nor <c>TYPESAFE_BASE_URL</c> is set.</summary>
    public const string DefaultBaseUrl = "https://api.typesafe.ai";

    /// <summary>Model used when a request omits <c>model</c>.</summary>
    public const string DefaultModelName = "jev-latest";

    private readonly string _apiKey;
    private readonly HttpClient _http;
    private readonly bool _ownsHttp;
    private readonly IReadOnlyDictionary<string, string> _defaultHeaders;
    private int _requestCount;

    /// <inheritdoc />
    public string BaseUrl { get; }

    /// <inheritdoc />
    public string DefaultModel { get; }

    /// <inheritdoc />
    public TypeSafeLogLevel LogLevel { get; }

    /// <summary>The configured logger, filtered to <see cref="LogLevel"/>.</summary>
    public ITypeSafeLogger Logger { get; }

    /// <inheritdoc />
    public RetryPolicy Retry { get; }

    /// <inheritdoc />
    public int TimeoutMs { get; }

    /// <summary>Additional headers sent with each request.</summary>
    public IReadOnlyDictionary<string, string> DefaultHeaders => _defaultHeaders;

    /// <inheritdoc />
    public ModelsResource Models { get; }

    /// <summary>
    /// Create a client. Explicit options take precedence over environment variables, then SDK defaults.
    /// Empty or whitespace-only environment values are ignored.
    /// </summary>
    /// <exception cref="TypeSafeException">The API key is missing or configuration is invalid.</exception>
    public TypeSafeClient(TypeSafeClientOptions? options = null)
        : this(httpClient: null, options, ownsHttpClient: null)
    {
    }

    /// <summary>Create a client that uses the given <see cref="HttpClient"/> (typically from <c>IHttpClientFactory</c>).</summary>
    public TypeSafeClient(HttpClient httpClient, TypeSafeClientOptions? options = null)
        : this(httpClient, options, ownsHttpClient: false)
    {
    }

    /// <summary>Create a client with an explicit API key.</summary>
    public TypeSafeClient(string apiKey, TypeSafeClientOptions? options = null)
        : this(httpClient: null, WithApiKey(options, apiKey), ownsHttpClient: null)
    {
    }

    /// <summary>Create a client with an explicit API key and <see cref="HttpClient"/>.</summary>
    public TypeSafeClient(string apiKey, HttpClient httpClient, TypeSafeClientOptions? options = null)
        : this(httpClient, WithApiKey(options, apiKey), ownsHttpClient: false)
    {
    }

    private TypeSafeClient(HttpClient? httpClient, TypeSafeClientOptions? options, bool? ownsHttpClient)
    {
        options ??= new TypeSafeClientOptions();
        _apiKey = Env.FromCodeOrEnv(options.ApiKey, Env.ApiKey)
            ?? throw new TypeSafeException(
                $"No API key was provided. Pass `ApiKey` to the TypeSafeClient constructor or set the {Env.ApiKey} environment variable.");

        BaseUrl = StripTrailingSlashes(Env.FromCodeOrEnv(options.BaseUrl, Env.BaseUrl) ?? DefaultBaseUrl);
        DefaultModel = Env.FromCodeOrEnv(options.DefaultModel, Env.DefaultModel) ?? DefaultModelName;
        LogLevel = ResolveLogLevel(options);
        Logger = new LevelFilteredLogger(options.Logger ?? new ConsoleTypeSafeLogger(), LogLevel);
        Retry = RetryPolicy.Resolve(RetryPolicy.Default, options.Retry).Validate();
        TimeoutMs = Guards.PositiveMs("timeout", options.TimeoutMs ?? RetryCalculator.DefaultTimeoutMs);
        _defaultHeaders = options.DefaultHeaders is null
            ? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            : new Dictionary<string, string>(options.DefaultHeaders, StringComparer.OrdinalIgnoreCase);

        if (httpClient is null)
        {
            _http = new HttpClient(options.HttpMessageHandler ?? new HttpClientHandler(), disposeHandler: options.HttpMessageHandler is null)
            {
                Timeout = Timeout.InfiniteTimeSpan,
            };
            _ownsHttp = true;
        }
        else
        {
            _http = httpClient;
            _ownsHttp = ownsHttpClient ?? false;
        }

        Models = new ModelsResource(this);
    }

    /// <inheritdoc />
    public async Task<SystemOneResult> SystemOneAsync(
        SystemOneRequest request,
        RequestOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var payload = AnswerParser.BuildSystemOnePayload(request, DefaultModel);
        return await SendAsync(
            HttpMethod.Post,
            "/v1/systemone",
            payload,
            options,
            cancellationToken,
            AnswerParser.ParseSystemOne).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public Task<TypeSafeResponse<SystemOneResult>> SystemOneWithResponseAsync(
        SystemOneRequest request,
        RequestOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var payload = AnswerParser.BuildSystemOnePayload(request, DefaultModel);
        return SendWithResponseAsync(
            HttpMethod.Post,
            "/v1/systemone",
            payload,
            options,
            cancellationToken,
            AnswerParser.ParseSystemOne);
    }

    /// <summary>Answer named questions and return the HTTP response metadata.</summary>
    public Task<TypeSafeResponse<SystemOneResult>> SystemOneWithResponseAsync(
        object? state,
        IDictionary<string, Question> questions,
        string? model = null,
        RequestOptions? options = null,
        CancellationToken cancellationToken = default) =>
        SystemOneWithResponseAsync(new SystemOneRequest { State = state, Questions = questions, Model = model }, options, cancellationToken);

    /// <summary>Answer named questions about text or structured state.</summary>
    public Task<SystemOneResult> SystemOneAsync(
        object? state,
        IDictionary<string, Question> questions,
        string? model = null,
        RequestOptions? options = null,
        CancellationToken cancellationToken = default) =>
        SystemOneAsync(new SystemOneRequest { State = state, Questions = questions, Model = model }, options, cancellationToken);

    internal async Task<T> SendAsync<T>(
        HttpMethod method,
        string path,
        JsonNode? body,
        RequestOptions? options,
        CancellationToken cancellationToken,
        Func<object?, T> parse)
    {
        var result = await SendCoreAsync(method, path, body, options, cancellationToken, parse).ConfigureAwait(false);
        return result.Data;
    }

    internal async Task<TypeSafeResponse<T>> SendWithResponseAsync<T>(
        HttpMethod method,
        string path,
        JsonNode? body,
        RequestOptions? options,
        CancellationToken cancellationToken,
        Func<object?, T> parse) =>
        await SendCoreAsync(method, path, body, options, cancellationToken, parse).ConfigureAwait(false);

    private async Task<TypeSafeResponse<T>> SendCoreAsync<T>(
        HttpMethod method,
        string path,
        JsonNode? body,
        RequestOptions? options,
        CancellationToken cancellationToken,
        Func<object?, T> parse)
    {
        options ??= new RequestOptions();
        var timeout = options.TimeoutMs is int perCall
            ? Guards.PositiveMs("timeout", perCall)
            : TimeoutMs;
        var retry = RetryPolicy.Resolve(Retry, options.Retry);
        if (options.Retry is not null)
        {
            retry.Validate();
        }

        using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, options.CancellationToken);
        var token = linked.Token;
        var headers = MergeHeaders(_defaultHeaders, options.Headers);
        var tag = $"#{Interlocked.Increment(ref _requestCount)} {method.Method} {path}";
        var url = BaseUrl + path;
        var json = body?.ToJsonString(JsonValues.SerializerOptions);

        HttpResponseMessage? response = null;
        object? parsedBody = null;
        long responseElapsedMs = 0;
        for (var attempt = 0; ; attempt++)
        {
            var retriesLeft = retry.MaxRetries - attempt;
            try
            {
                var attemptResult = await AttemptAsync(tag, url, method, json, headers, attempt, timeout, token).ConfigureAwait(false);
                response = attemptResult.Response;
                responseElapsedMs = attemptResult.ElapsedMs;
            }
            catch (ApiUserAbortException)
            {
                throw;
            }
            catch (Exception ex) when (ex is ApiTimeoutException or ApiConnectionException)
            {
                if (retriesLeft <= 0 || !IsRetryableError(ex, retry))
                {
                    throw;
                }

                await BackOffAsync(tag, attempt, retriesLeft, ex.Message, headers: null, retry, token).ConfigureAwait(false);
                continue;
            }

            var requestId = ApiException.TryGetRequestId(response.Headers);
            Logger.Info($"{tag} <- {(int)response.StatusCode} in {responseElapsedMs}ms{(requestId is null ? "" : $" (request {requestId})")}");

            var text = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            parsedBody = JsonValues.ParseBody(text, response.Content.Headers.ContentType?.MediaType);

            if (response.IsSuccessStatusCode)
            {
                Logger.Debug($"{tag} <- body", parsedBody);
                var data = parse(parsedBody);
                return new TypeSafeResponse<T>(data, response, requestId);
            }

            Logger.Debug($"{tag} <- error body", parsedBody);
            var error = ApiException.FromResponse(response.StatusCode, parsedBody, response.Headers);
            if (retriesLeft <= 0 || !RetryCalculator.IsRetryableStatus(error.Status, retry))
            {
                response.Dispose();
                throw error;
            }

            var errorHeaders = response.Headers;
            response.Dispose();
            await BackOffAsync(tag, attempt, retriesLeft, $"{error.Status}", errorHeaders, retry, token).ConfigureAwait(false);
        }
    }

    private async Task<(HttpResponseMessage Response, long ElapsedMs)> AttemptAsync(
        string tag,
        string url,
        HttpMethod method,
        string? json,
        IReadOnlyDictionary<string, string> userHeaders,
        int attempt,
        int timeout,
        CancellationToken callerToken)
    {
        using var attemptCts = CancellationTokenSource.CreateLinkedTokenSource(callerToken);
        attemptCts.CancelAfter(timeout);

        using var request = new HttpRequestMessage(method, url);
        foreach (var header in BuildHeaders(userHeaders, json is not null, attempt))
        {
            if (header.Key.Equals("Content-Type", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            request.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        if (json is not null)
        {
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        Logger.Debug($"{tag} -> {url}", new
        {
            headers = HeaderRedactor.Redact(BuildHeaders(userHeaders, json is not null, attempt)),
            body = json is null ? null : JsonNode.Parse(json),
        });

        var started = Stopwatch.StartNew();
        try
        {
            var response = await _http.SendAsync(request, HttpCompletionOption.ResponseContentRead, attemptCts.Token)
                .ConfigureAwait(false);
            return (response, started.ElapsedMilliseconds);
        }
        catch (OperationCanceledException ex) when (callerToken.IsCancellationRequested)
        {
            Logger.Info($"{tag} aborted by caller after {started.ElapsedMilliseconds}ms");
            throw new ApiUserAbortException(innerException: ex);
        }
        catch (OperationCanceledException ex) when (attemptCts.IsCancellationRequested)
        {
            Logger.Info($"{tag} timed out after {started.ElapsedMilliseconds}ms");
            throw new ApiTimeoutException(timeout, ex);
        }
        catch (HttpRequestException ex)
        {
            Logger.Info($"{tag} connection error after {started.ElapsedMilliseconds}ms", ex);
            throw new ApiConnectionException($"Connection error: {ex.Message}", ex);
        }
        catch (Exception ex) when (ex is not TypeSafeException)
        {
            Logger.Info($"{tag} connection error after {started.ElapsedMilliseconds}ms", ex);
            throw new ApiConnectionException($"Connection error: {ex.Message}", ex);
        }
    }

    private async Task BackOffAsync(
        string tag,
        int attempt,
        int retriesLeft,
        string reason,
        HttpResponseHeaders? headers,
        RetryPolicy retry,
        CancellationToken token)
    {
        var delay = RetryCalculator.DelayMs(attempt, headers, retry);
        Logger.Info($"{tag} retrying in {delay}ms (retry {attempt + 1}/{attempt + retriesLeft}) after {reason}");
        try
        {
            await RetryCalculator.SleepAsync(delay, token).ConfigureAwait(false);
        }
        catch (OperationCanceledException ex)
        {
            Logger.Info($"{tag} aborted by caller while waiting to retry");
            throw new ApiUserAbortException(innerException: ex);
        }
    }

    private Dictionary<string, string> BuildHeaders(IReadOnlyDictionary<string, string> userHeaders, bool hasBody, int attempt)
    {
        var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var pair in userHeaders)
        {
            headers[pair.Key] = pair.Value;
        }

        // User-supplied headers go first so they can't clobber auth or the JSON content type.
        headers["Authorization"] = $"Bearer {_apiKey}";
        headers["Accept"] = "application/json";
        headers["User-Agent"] = $"typesafe-sdk-dotnet/{SdkVersion.Current}";
        headers["X-TypeSafe-SDK"] = $"typesafe-sdk-dotnet/{SdkVersion.Current}";
        headers["X-TypeSafe-Runtime"] = RuntimeInfo.Description;
        if (hasBody)
        {
            headers["Content-Type"] = "application/json";
        }

        if (attempt > 0)
        {
            headers["X-TypeSafe-Retry-Count"] = attempt.ToString();
        }

        return headers;
    }

    private static Dictionary<string, string> MergeHeaders(
        IReadOnlyDictionary<string, string> defaults,
        IDictionary<string, string>? perCall)
    {
        var merged = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var pair in defaults)
        {
            merged[pair.Key] = pair.Value;
        }
        if (perCall is null)
        {
            return merged;
        }

        foreach (var pair in perCall)
        {
            merged[pair.Key] = pair.Value;
        }

        return merged;
    }

    private static bool IsRetryableError(Exception error, RetryPolicy policy) =>
        error switch
        {
            ApiTimeoutException => policy.ApiTimeoutError,
            ApiConnectionException => policy.ApiConnectionError,
            _ => false,
        };

    private static TypeSafeLogLevel ResolveLogLevel(TypeSafeClientOptions options)
    {
        if (options.LogLevel is TypeSafeLogLevel fromCode)
        {
            return fromCode;
        }

        if (!string.IsNullOrWhiteSpace(options.LogLevelText))
        {
            return LogLevels.Parse(options.LogLevelText!, "the `logLevel` option");
        }

        var fromEnv = Env.Read(Env.LogLevel);
        return fromEnv is null ? LogLevels.Default : LogLevels.Parse(fromEnv, Env.LogLevel);
    }

    private static string StripTrailingSlashes(string url)
    {
        return url.TrimEnd('/');
    }

    private static TypeSafeClientOptions WithApiKey(TypeSafeClientOptions? options, string apiKey)
    {
        options ??= new TypeSafeClientOptions();
        options.ApiKey = apiKey;
        return options;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (_ownsHttp)
        {
            _http.Dispose();
        }
    }
}
