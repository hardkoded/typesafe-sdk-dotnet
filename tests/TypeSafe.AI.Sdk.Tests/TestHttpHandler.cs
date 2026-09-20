using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace TypeSafe.AI.Sdk.Tests;

internal sealed class RecordedRequest
{
    public required HttpRequestMessage Message { get; init; }
    public required string Url { get; init; }
    public required string Method { get; init; }
    public required string? BodyText { get; init; }
    public JsonElement? Body { get; init; }
    public Dictionary<string, string> Headers { get; } = new(StringComparer.OrdinalIgnoreCase);
}

internal sealed class ScriptedHandler : HttpMessageHandler
{
    private readonly Func<RecordedRequest, int, CancellationToken, Task<HttpResponseMessage>> _respond;
    public List<RecordedRequest> Requests { get; } = new();

    public ScriptedHandler(Func<RecordedRequest, int, HttpResponseMessage> respond)
        : this((req, n, _) => Task.FromResult(respond(req, n)))
    {
    }

    public ScriptedHandler(Func<RecordedRequest, int, CancellationToken, Task<HttpResponseMessage>> respond)
    {
        _respond = respond;
    }

    public static ScriptedHandler Hang() =>
        new(async (_, _, cancellationToken) =>
        {
            await Task.Delay(Timeout.Infinite, cancellationToken).ConfigureAwait(false);
            return new HttpResponseMessage(HttpStatusCode.OK);
        });

    public static ScriptedHandler Json(
        object body,
        HttpStatusCode status = HttpStatusCode.OK,
        IDictionary<string, string>? headers = null) =>
        Sequence(JsonResponse(body, status, headers));

    public static ScriptedHandler Sequence(params HttpResponseMessage[] responses)
    {
        var index = 0;
        return new ScriptedHandler((_, _) =>
        {
            var current = index < responses.Length ? responses[index] : responses[^1];
            index++;
            return Clone(current);
        });
    }

    public static HttpResponseMessage JsonResponse(
        object? body,
        HttpStatusCode status = HttpStatusCode.OK,
        IDictionary<string, string>? headers = null)
    {
        var json = body is null || (body is string s && s.Length == 0)
            ? ""
            : body is string text && !LooksLikeJson(text)
                ? text
                : body is string jsonText
                    ? jsonText
                    : JsonSerializer.Serialize(body);
        var response = new HttpResponseMessage(status)
        {
            Content = new StringContent(json, Encoding.UTF8, body is string raw && !LooksLikeJson(raw) ? "text/html" : "application/json"),
        };
        if (body is string plain && !LooksLikeJson(plain))
        {
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/html");
        }

        if (headers is not null)
        {
            foreach (var pair in headers)
            {
                if (!response.Headers.TryAddWithoutValidation(pair.Key, pair.Value))
                {
                    response.Content.Headers.TryAddWithoutValidation(pair.Key, pair.Value);
                }
            }
        }

        return response;
    }

    public static HttpResponseMessage Empty(HttpStatusCode status, IDictionary<string, string>? headers = null)
    {
        var response = new HttpResponseMessage(status) { Content = new StringContent("") };
        if (headers is not null)
        {
            foreach (var pair in headers)
            {
                response.Headers.TryAddWithoutValidation(pair.Key, pair.Value);
            }
        }

        return response;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var bodyText = request.Content is null ? null : await request.Content.ReadAsStringAsync().ConfigureAwait(false);
        JsonElement? body = null;
        if (!string.IsNullOrEmpty(bodyText))
        {
            try
            {
                body = JsonSerializer.Deserialize<JsonElement>(bodyText);
            }
            catch (JsonException)
            {
                // leave null
            }
        }

        var recorded = new RecordedRequest
        {
            Message = request,
            Url = request.RequestUri?.ToString() ?? "",
            Method = request.Method.Method,
            BodyText = bodyText,
            Body = body,
        };
        foreach (var header in request.Headers)
        {
            recorded.Headers[header.Key] = string.Join(",", header.Value);
        }

        if (request.Content is not null)
        {
            foreach (var header in request.Content.Headers)
            {
                recorded.Headers[header.Key] = string.Join(",", header.Value);
            }
        }

        Requests.Add(recorded);
        return await _respond(recorded, Requests.Count, cancellationToken).ConfigureAwait(false);
    }

    private static bool LooksLikeJson(string text)
    {
        var trimmed = text.TrimStart();
        return trimmed.StartsWith("{") || trimmed.StartsWith("[") || trimmed.StartsWith("\"");
    }

    private static HttpResponseMessage Clone(HttpResponseMessage source)
    {
        var clone = new HttpResponseMessage(source.StatusCode)
        {
            Content = new StringContent(source.Content.ReadAsStringAsync().GetAwaiter().GetResult(), Encoding.UTF8, source.Content.Headers.ContentType?.MediaType ?? "application/json"),
            ReasonPhrase = source.ReasonPhrase,
        };
        foreach (var header in source.Headers)
        {
            clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        return clone;
    }
}

internal static class TestClient
{
    public static TypeSafeClient Create(
        ScriptedHandler handler,
        Action<TypeSafeClientOptions>? configure = null)
    {
        var options = new TypeSafeClientOptions
        {
            ApiKey = "k",
            HttpMessageHandler = handler,
            Logger = new RecordingLogger(),
            LogLevel = TypeSafeLogLevel.Off,
        };
        configure?.Invoke(options);
        options.HttpMessageHandler = handler;
        return new TypeSafeClient(options);
    }
}

internal sealed class RecordingLogger : ITypeSafeLogger
{
    public List<string> Lines { get; } = new();

    public void Debug(string message, params object?[] args) => Lines.Add("debug " + message);

    public void Info(string message, params object?[] args) => Lines.Add(message);

    public void Warn(string message, params object?[] args) => Lines.Add("warn " + message);

    public void Error(string message, params object?[] args) => Lines.Add("error " + message);
}

internal static class Fixtures
{
    public static readonly object SystemOne = new
    {
        model = "m",
        answers = new { q1 = new { type = "noul", noul = 0.5 } },
        usage = new { input_tokens = 1, output_tokens = 1 },
    };

    public static readonly object Models = new
    {
        models = new[] { new { name = "m", description = "d", release_date = "2026" } },
    };
}
