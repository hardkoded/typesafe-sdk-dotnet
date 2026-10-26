---
title: "RateLimitException"
description: "HTTP 429: the rate limit was exceeded."
editUrl: false
prev: false
next: false
sidebar:
  label: "RateLimitException"
  hidden: true
---

<a id="TypeSafe_AI_Sdk_RateLimitException"></a>

Namespace: [TypeSafe.AI.Sdk](/api/TypeSafe.AI.Sdk/)


```csharp
public class RateLimitException : ApiException, ISerializable
```


HTTP 429: the rate limit was exceeded.

**Inheritance:** [object](https://learn.microsoft.com/dotnet/api/system.object) → [Exception](https://learn.microsoft.com/dotnet/api/system.exception) → [TypeSafeException](/api/TypeSafe.AI.Sdk/TypeSafeException/) → [ApiException](/api/TypeSafe.AI.Sdk/ApiException/)

**Implements:** [ISerializable](https://learn.microsoft.com/dotnet/api/system.runtime.serialization.iserializable)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Errors/TypeSafeExceptions.cs#L247)

## Constructors

<a id="TypeSafe_AI_Sdk_RateLimitException__ctor_"></a>
<a id="TypeSafe_AI_Sdk_RateLimitException__ctor_System_Net_HttpStatusCode_System_Object_System_Net_Http_Headers_HttpResponseHeaders_System_String_"></a>

### RateLimitException(HttpStatusCode, object?, HttpResponseHeaders, string?)


```csharp
public RateLimitException(HttpStatusCode statusCode, object? body, HttpResponseHeaders headers, string? message = null)
```


Create an API error from a completed HTTP response.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `statusCode` | [HttpStatusCode](https://learn.microsoft.com/dotnet/api/system.net.httpstatuscode) |  |
| `body` | [object](https://learn.microsoft.com/dotnet/api/system.object) |  |
| `headers` | [HttpResponseHeaders](https://learn.microsoft.com/dotnet/api/system.net.http.headers.httpresponseheaders) |  |
| `message` | [string](https://learn.microsoft.com/dotnet/api/system.string) |  |

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Errors/TypeSafeExceptions.cs#L253)

## Properties

<a id="TypeSafe_AI_Sdk_RateLimitException_RetryAfterMs_"></a>
<a id="TypeSafe_AI_Sdk_RateLimitException_RetryAfterMs"></a>

### RetryAfterMs


```csharp
public int? RetryAfterMs { get; }
```


Server retry delay in milliseconds, or `null` when absent or invalid.

**Value:** [int?](https://learn.microsoft.com/dotnet/api/system.int32)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Errors/TypeSafeExceptions.cs#L250)
