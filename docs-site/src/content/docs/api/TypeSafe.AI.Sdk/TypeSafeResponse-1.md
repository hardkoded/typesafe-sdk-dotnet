---
title: "TypeSafeResponse<T>"
description: "Parsed data with its HTTP response and request ID. Mirrors JS WithResponse."
editUrl: false
prev: false
next: false
sidebar:
  label: "TypeSafeResponse<T>"
  hidden: true
---

<a id="TypeSafe_AI_Sdk_TypeSafeResponse_1"></a>

Namespace: [TypeSafe.AI.Sdk](/api/TypeSafe.AI.Sdk/)


```csharp
public sealed class TypeSafeResponse<T>
```


Parsed data with its HTTP response and request ID. Mirrors JS `WithResponse`.

**Type parameters**

| Name | Description |
| --- | --- |
| `T` |  |

**Inheritance:** [object](https://learn.microsoft.com/dotnet/api/system.object)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Responses/Responses.cs#L165)

## Constructors

<a id="TypeSafe_AI_Sdk_TypeSafeResponse_1__ctor_"></a>
<a id="TypeSafe_AI_Sdk_TypeSafeResponse_1__ctor__0_System_Net_Http_HttpResponseMessage_System_String_"></a>

### TypeSafeResponse(T, HttpResponseMessage, string?)


```csharp
public TypeSafeResponse(T data, HttpResponseMessage response, string? requestId)
```


Create a typed response wrapper.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `data` | `T` |  |
| `response` | [HttpResponseMessage](https://learn.microsoft.com/dotnet/api/system.net.http.httpresponsemessage) |  |
| `requestId` | [string](https://learn.microsoft.com/dotnet/api/system.string) |  |

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Responses/Responses.cs#L177)

## Properties

<a id="TypeSafe_AI_Sdk_TypeSafeResponse_1_Data_"></a>
<a id="TypeSafe_AI_Sdk_TypeSafeResponse_1_Data"></a>

### Data


```csharp
public T Data { get; }
```


The parsed response body.

**Value:** `T`

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Responses/Responses.cs#L168)

<a id="TypeSafe_AI_Sdk_TypeSafeResponse_1_RequestId_"></a>
<a id="TypeSafe_AI_Sdk_TypeSafeResponse_1_RequestId"></a>

### RequestId


```csharp
public string? RequestId { get; }
```


Request ID from `x-typesafe-request-id`, or `null` when absent.

**Value:** [string](https://learn.microsoft.com/dotnet/api/system.string)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Responses/Responses.cs#L174)

<a id="TypeSafe_AI_Sdk_TypeSafeResponse_1_Response_"></a>
<a id="TypeSafe_AI_Sdk_TypeSafeResponse_1_Response"></a>

### Response


```csharp
public HttpResponseMessage Response { get; }
```


The HTTP response, with its body already consumed by parsing.

**Value:** [HttpResponseMessage](https://learn.microsoft.com/dotnet/api/system.net.http.httpresponsemessage)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Responses/Responses.cs#L171)
