---
title: "ApiConnectionException"
description: "The request or response-body delivery failed (DNS, TLS, connection closed, etc.). Mirrors JS APIConnectionError."
editUrl: false
prev: false
next: false
sidebar:
  label: "ApiConnectionException"
  hidden: true
---

<a id="TypeSafe_AI_Sdk_ApiConnectionException"></a>

Namespace: [TypeSafe.AI.Sdk](/api/TypeSafe.AI.Sdk/)


```csharp
public class ApiConnectionException : TypeSafeException, ISerializable
```


The request or response-body delivery failed (DNS, TLS, connection closed, etc.).
Mirrors JS `APIConnectionError`.

**Inheritance:** [object](https://learn.microsoft.com/dotnet/api/system.object) → [Exception](https://learn.microsoft.com/dotnet/api/system.exception) → [TypeSafeException](/api/TypeSafe.AI.Sdk/TypeSafeException/)

**Implements:** [ISerializable](https://learn.microsoft.com/dotnet/api/system.runtime.serialization.iserializable)

**Derived:** [ApiTimeoutException](/api/TypeSafe.AI.Sdk/ApiTimeoutException/)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Errors/TypeSafeExceptions.cs#L274)

## Constructors

<a id="TypeSafe_AI_Sdk_ApiConnectionException__ctor_"></a>
<a id="TypeSafe_AI_Sdk_ApiConnectionException__ctor_System_String_System_Exception_"></a>

### ApiConnectionException(string?, Exception?)


```csharp
public ApiConnectionException(string? message = null, Exception? innerException = null)
```


Create a connection error.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `message` | [string](https://learn.microsoft.com/dotnet/api/system.string) |  |
| `innerException` | [Exception](https://learn.microsoft.com/dotnet/api/system.exception) |  |

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Errors/TypeSafeExceptions.cs#L277)
