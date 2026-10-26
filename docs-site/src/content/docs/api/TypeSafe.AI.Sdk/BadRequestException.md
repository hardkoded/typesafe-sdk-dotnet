---
title: "BadRequestException"
description: "HTTP 400: the request is invalid."
editUrl: false
prev: false
next: false
sidebar:
  label: "BadRequestException"
  hidden: true
---

<a id="TypeSafe_AI_Sdk_BadRequestException"></a>

Namespace: [TypeSafe.AI.Sdk](/api/TypeSafe.AI.Sdk/)


```csharp
public class BadRequestException : ApiException, ISerializable
```


HTTP 400: the request is invalid.

**Inheritance:** [object](https://learn.microsoft.com/dotnet/api/system.object) → [Exception](https://learn.microsoft.com/dotnet/api/system.exception) → [TypeSafeException](/api/TypeSafe.AI.Sdk/TypeSafeException/) → [ApiException](/api/TypeSafe.AI.Sdk/ApiException/)

**Implements:** [ISerializable](https://learn.microsoft.com/dotnet/api/system.runtime.serialization.iserializable)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Errors/TypeSafeExceptions.cs#L197)

## Constructors

<a id="TypeSafe_AI_Sdk_BadRequestException__ctor_"></a>
<a id="TypeSafe_AI_Sdk_BadRequestException__ctor_System_Net_HttpStatusCode_System_Object_System_Net_Http_Headers_HttpResponseHeaders_System_String_"></a>

### BadRequestException(HttpStatusCode, object?, HttpResponseHeaders, string?)


```csharp
public BadRequestException(HttpStatusCode statusCode, object? body, HttpResponseHeaders headers, string? message = null)
```


Create an API error from a completed HTTP response.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `statusCode` | [HttpStatusCode](https://learn.microsoft.com/dotnet/api/system.net.httpstatuscode) |  |
| `body` | [object](https://learn.microsoft.com/dotnet/api/system.object) |  |
| `headers` | [HttpResponseHeaders](https://learn.microsoft.com/dotnet/api/system.net.http.headers.httpresponseheaders) |  |
| `message` | [string](https://learn.microsoft.com/dotnet/api/system.string) |  |

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Errors/TypeSafeExceptions.cs#L200)
