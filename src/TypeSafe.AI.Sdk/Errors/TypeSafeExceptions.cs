// Copyright (c) Dario Kondratiuk.
// Licensed under the MIT License.

using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace TypeSafe.AI.Sdk;

/// <summary>Base class for SDK errors. Mirrors JS <c>TypeSafeError</c>.</summary>
public class TypeSafeException : Exception
{
    /// <summary>Create an SDK error.</summary>
    public TypeSafeException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}

/// <summary>An unsuccessful HTTP response from the API. Mirrors JS <c>APIError</c>.</summary>
public class ApiException : TypeSafeException
{
    internal const string RequestIdHeader = "x-typesafe-request-id";
    private const int MaxRawBodyInMessage = 200;

    /// <summary>HTTP response status code.</summary>
    public HttpStatusCode StatusCode { get; }

    /// <summary>Numeric HTTP status.</summary>
    public int Status => (int)StatusCode;

    /// <summary>HTTP response headers.</summary>
    public HttpResponseHeaders Headers { get; }

    /// <summary>Parsed JSON, response text, or <c>null</c> for an empty body.</summary>
    public object? Body { get; }

    /// <summary>Request ID from <c>x-typesafe-request-id</c>, or <c>null</c> when absent.</summary>
    public string? RequestId { get; }

    /// <summary>Create an API error from a completed HTTP response.</summary>
    public ApiException(
        HttpStatusCode statusCode,
        object? body,
        HttpResponseHeaders headers,
        string? message = null)
        : base(message ?? Describe(statusCode, body))
    {
        StatusCode = statusCode;
        Body = body;
        Headers = headers;
        RequestId = TryGetRequestId(headers);
    }

    /// <summary>Create the error subclass for an HTTP status code.</summary>
    public static ApiException FromResponse(HttpStatusCode statusCode, object? body, HttpResponseHeaders headers)
    {
        var status = (int)statusCode;
        return status switch
        {
            400 => new BadRequestException(statusCode, body, headers),
            401 => new AuthenticationException(statusCode, body, headers),
            403 => new PermissionDeniedException(statusCode, body, headers),
            404 => new NotFoundException(statusCode, body, headers),
            422 => new UnprocessableEntityException(statusCode, body, headers),
            429 => new RateLimitException(statusCode, body, headers),
            >= 500 => new InternalServerException(statusCode, body, headers),
            _ => new ApiException(statusCode, body, headers),
        };
    }

    internal static string? TryGetRequestId(HttpResponseHeaders headers)
    {
        if (headers.TryGetValues(RequestIdHeader, out var values))
        {
            return values.FirstOrDefault();
        }

        return null;
    }

    private static string Describe(HttpStatusCode statusCode, object? body)
    {
        var status = (int)statusCode;
        var detail = ExtractMessage(body);
        if (detail is not null)
        {
            return $"{status} {detail}";
        }

        if (body is null)
        {
            return $"{status} status code (no body)";
        }

        var raw = body as string ?? JsonSerializer.Serialize(body);
        if (raw.Length > MaxRawBodyInMessage)
        {
#if NET
            raw = string.Concat(raw.AsSpan(0, MaxRawBodyInMessage), "…");
#else
            raw = raw.Substring(0, MaxRawBodyInMessage) + "…";
#endif
        }

        return $"{status} {raw}";
    }

    internal static string? ExtractMessage(object? body)
    {
        if (body is string text)
        {
            return string.IsNullOrEmpty(text) ? null : text;
        }

        if (body is not JsonElement element || element.ValueKind != JsonValueKind.Object)
        {
            return null;
        }

        if (TryGetString(element, "error", out var errorString))
        {
            return errorString;
        }

        if (element.TryGetProperty("error", out var error) && error.ValueKind == JsonValueKind.Object
            && TryGetString(error, "message", out var errorMessage))
        {
            return errorMessage;
        }

        if (TryGetString(element, "message", out var message))
        {
            return message;
        }

        if (TryGetString(element, "detail", out var detail))
        {
            return detail;
        }

        if (element.TryGetProperty("detail", out var detailNode))
        {
            if (detailNode.ValueKind == JsonValueKind.Object
                && TryGetString(detailNode, "message", out var detailMessage))
            {
                return detailMessage;
            }

            if (detailNode.ValueKind == JsonValueKind.Array)
            {
                return DescribeValidationErrors(detailNode);
            }
        }

        return null;
    }

    private static string? DescribeValidationErrors(JsonElement errors)
    {
        var parts = new List<string>();
        foreach (var item in errors.EnumerateArray())
        {
            if (item.ValueKind != JsonValueKind.Object || !TryGetString(item, "msg", out var msg) || msg is null)
            {
                continue;
            }

            var loc = "";
            if (item.TryGetProperty("loc", out var locNode) && locNode.ValueKind == JsonValueKind.Array)
            {
                loc = string.Join(".", locNode.EnumerateArray()
                    .Select(x => x.ValueKind == JsonValueKind.String ? x.GetString() : x.ToString())
                    .Where(x => !string.IsNullOrEmpty(x) && x != "body"));
            }

            parts.Add(string.IsNullOrEmpty(loc) ? msg : $"{loc}: {msg}");
        }

        return parts.Count == 0 ? null : string.Join("; ", parts);
    }

    private static bool TryGetString(JsonElement element, string name, out string? value)
    {
        value = null;
        if (!element.TryGetProperty(name, out var property) || property.ValueKind != JsonValueKind.String)
        {
            return false;
        }

        value = property.GetString();
        return value is not null;
    }
}

/// <summary>HTTP 400: the request is invalid.</summary>
public class BadRequestException : ApiException
{
    /// <inheritdoc />
    public BadRequestException(HttpStatusCode statusCode, object? body, HttpResponseHeaders headers, string? message = null)
        : base(statusCode, body, headers, message)
    {
    }
}

/// <summary>HTTP 401: authentication failed.</summary>
public class AuthenticationException : ApiException
{
    /// <inheritdoc />
    public AuthenticationException(HttpStatusCode statusCode, object? body, HttpResponseHeaders headers, string? message = null)
        : base(statusCode, body, headers, message)
    {
    }
}

/// <summary>HTTP 403: access is denied.</summary>
public class PermissionDeniedException : ApiException
{
    /// <inheritdoc />
    public PermissionDeniedException(HttpStatusCode statusCode, object? body, HttpResponseHeaders headers, string? message = null)
        : base(statusCode, body, headers, message)
    {
    }
}

/// <summary>HTTP 404: the resource was not found.</summary>
public class NotFoundException : ApiException
{
    /// <inheritdoc />
    public NotFoundException(HttpStatusCode statusCode, object? body, HttpResponseHeaders headers, string? message = null)
        : base(statusCode, body, headers, message)
    {
    }
}

/// <summary>HTTP 422: request validation failed.</summary>
public class UnprocessableEntityException : ApiException
{
    /// <inheritdoc />
    public UnprocessableEntityException(HttpStatusCode statusCode, object? body, HttpResponseHeaders headers, string? message = null)
        : base(statusCode, body, headers, message)
    {
    }
}

/// <summary>HTTP 429: the rate limit was exceeded.</summary>
public class RateLimitException : ApiException
{
    /// <summary>Server retry delay in milliseconds, or <c>null</c> when absent or invalid.</summary>
    public int? RetryAfterMs { get; }

    /// <inheritdoc />
    public RateLimitException(HttpStatusCode statusCode, object? body, HttpResponseHeaders headers, string? message = null)
        : base(statusCode, body, headers, message)
    {
        RetryAfterMs = RetryCalculator.ParseRetryAfter(headers);
    }
}

/// <summary>HTTP 5xx: the server failed to handle the request.</summary>
public class InternalServerException : ApiException
{
    /// <inheritdoc />
    public InternalServerException(HttpStatusCode statusCode, object? body, HttpResponseHeaders headers, string? message = null)
        : base(statusCode, body, headers, message)
    {
    }
}

/// <summary>
/// The request or response-body delivery failed (DNS, TLS, connection closed, etc.).
/// Mirrors JS <c>APIConnectionError</c>.
/// </summary>
public class ApiConnectionException : TypeSafeException
{
    /// <summary>Create a connection error.</summary>
    public ApiConnectionException(string? message = null, Exception? innerException = null)
        : base(message ?? "Connection error.", innerException)
    {
    }
}

/// <summary>
/// The full response did not arrive within the timeout. A kind of <see cref="ApiConnectionException"/>.
/// Mirrors JS <c>APITimeoutError</c>.
/// </summary>
public class ApiTimeoutException : ApiConnectionException
{
    /// <summary>Configured timeout in milliseconds.</summary>
    public int TimeoutMs { get; }

    /// <summary>Create a timeout error.</summary>
    public ApiTimeoutException(int timeoutMs, Exception? innerException = null)
        : base($"Request timed out after {timeoutMs}ms.", innerException)
    {
        TimeoutMs = timeoutMs;
    }
}

/// <summary>
/// The caller cancelled the request through a <see cref="CancellationToken"/>.
/// Mirrors JS <c>APIUserAbortError</c>.
/// </summary>
public class ApiUserAbortException : TypeSafeException
{
    /// <summary>Create an abort error.</summary>
    public ApiUserAbortException(string? message = null, Exception? innerException = null)
        : base(message ?? "Request was aborted.", innerException)
    {
    }
}
