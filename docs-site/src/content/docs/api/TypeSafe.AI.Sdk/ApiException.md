---
title: "ApiException"
description: "An unsuccessful HTTP response from the API. Mirrors JS APIError."
editUrl: false
prev: false
next: false
sidebar:
  label: "ApiException"
  hidden: true
---

<a id="TypeSafe_AI_Sdk_ApiException"></a>

Namespace: [TypeSafe.AI.Sdk](/api/TypeSafe.AI.Sdk/)


```csharp
public class ApiException : TypeSafeException, ISerializable
```


An unsuccessful HTTP response from the API. Mirrors JS `APIError`.

**Inheritance:** [object](https://learn.microsoft.com/dotnet/api/system.object) → [Exception](https://learn.microsoft.com/dotnet/api/system.exception) → [TypeSafeException](/api/TypeSafe.AI.Sdk/TypeSafeException/)

**Implements:** [ISerializable](https://learn.microsoft.com/dotnet/api/system.runtime.serialization.iserializable)

**Derived:** [AuthenticationException](/api/TypeSafe.AI.Sdk/AuthenticationException/), [BadRequestException](/api/TypeSafe.AI.Sdk/BadRequestException/), [InternalServerException](/api/TypeSafe.AI.Sdk/InternalServerException/), [NotFoundException](/api/TypeSafe.AI.Sdk/NotFoundException/), [PermissionDeniedException](/api/TypeSafe.AI.Sdk/PermissionDeniedException/), [RateLimitException](/api/TypeSafe.AI.Sdk/RateLimitException/), [UnprocessableEntityException](/api/TypeSafe.AI.Sdk/UnprocessableEntityException/)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Errors/TypeSafeExceptions.cs#L21)

## Constructors

<a id="TypeSafe_AI_Sdk_ApiException__ctor_"></a>
<a id="TypeSafe_AI_Sdk_ApiException__ctor_System_Net_HttpStatusCode_System_Object_System_Net_Http_Headers_HttpResponseHeaders_System_String_"></a>

### ApiException(HttpStatusCode, object?, HttpResponseHeaders, string?)


```csharp
public ApiException(HttpStatusCode statusCode, object? body, HttpResponseHeaders headers, string? message = null)
```


Create an API error from a completed HTTP response.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `statusCode` | [HttpStatusCode](https://learn.microsoft.com/dotnet/api/system.net.httpstatuscode) |  |
| `body` | [object](https://learn.microsoft.com/dotnet/api/system.object) |  |
| `headers` | [HttpResponseHeaders](https://learn.microsoft.com/dotnet/api/system.net.http.headers.httpresponseheaders) |  |
| `message` | [string](https://learn.microsoft.com/dotnet/api/system.string) |  |

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Errors/TypeSafeExceptions.cs#L42)

## Properties

<a id="TypeSafe_AI_Sdk_ApiException_Body_"></a>
<a id="TypeSafe_AI_Sdk_ApiException_Body"></a>

### Body


```csharp
public object? Body { get; }
```


Parsed JSON, response text, or `null` for an empty body.

**Value:** [object](https://learn.microsoft.com/dotnet/api/system.object)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Errors/TypeSafeExceptions.cs#L36)

<a id="TypeSafe_AI_Sdk_ApiException_Headers_"></a>
<a id="TypeSafe_AI_Sdk_ApiException_Headers"></a>

### Headers


```csharp
public HttpResponseHeaders Headers { get; }
```


HTTP response headers.

**Value:** [HttpResponseHeaders](https://learn.microsoft.com/dotnet/api/system.net.http.headers.httpresponseheaders)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Errors/TypeSafeExceptions.cs#L33)

<a id="TypeSafe_AI_Sdk_ApiException_RequestId_"></a>
<a id="TypeSafe_AI_Sdk_ApiException_RequestId"></a>

### RequestId


```csharp
public string? RequestId { get; }
```


Request ID from `x-typesafe-request-id`, or `null` when absent.

**Value:** [string](https://learn.microsoft.com/dotnet/api/system.string)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Errors/TypeSafeExceptions.cs#L39)

<a id="TypeSafe_AI_Sdk_ApiException_Status_"></a>
<a id="TypeSafe_AI_Sdk_ApiException_Status"></a>

### Status


```csharp
public int Status { get; }
```


Numeric HTTP status.

**Value:** [int](https://learn.microsoft.com/dotnet/api/system.int32)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Errors/TypeSafeExceptions.cs#L30)

<a id="TypeSafe_AI_Sdk_ApiException_StatusCode_"></a>
<a id="TypeSafe_AI_Sdk_ApiException_StatusCode"></a>

### StatusCode


```csharp
public HttpStatusCode StatusCode { get; }
```


HTTP response status code.

**Value:** [HttpStatusCode](https://learn.microsoft.com/dotnet/api/system.net.httpstatuscode)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Errors/TypeSafeExceptions.cs#L27)

## Methods

<a id="TypeSafe_AI_Sdk_ApiException_FromResponse_"></a>
<a id="TypeSafe_AI_Sdk_ApiException_FromResponse_System_Net_HttpStatusCode_System_Object_System_Net_Http_Headers_HttpResponseHeaders_"></a>

### FromResponse(HttpStatusCode, object?, HttpResponseHeaders)


```csharp
public static ApiException FromResponse(HttpStatusCode statusCode, object? body, HttpResponseHeaders headers)
```


Create the error subclass for an HTTP status code.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `statusCode` | [HttpStatusCode](https://learn.microsoft.com/dotnet/api/system.net.httpstatuscode) |  |
| `body` | [object](https://learn.microsoft.com/dotnet/api/system.object) |  |
| `headers` | [HttpResponseHeaders](https://learn.microsoft.com/dotnet/api/system.net.http.headers.httpresponseheaders) |  |

**Returns:** [ApiException](./)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Errors/TypeSafeExceptions.cs#L56)
