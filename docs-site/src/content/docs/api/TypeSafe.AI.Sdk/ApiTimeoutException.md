---
title: "ApiTimeoutException"
description: "The full response did not arrive within the timeout. A kind of ApiConnectionException. Mirrors JS APITimeoutError."
editUrl: false
prev: false
next: false
sidebar:
  label: "ApiTimeoutException"
  hidden: true
---

<a id="TypeSafe_AI_Sdk_ApiTimeoutException"></a>

Namespace: [TypeSafe.AI.Sdk](/api/TypeSafe.AI.Sdk/)


```csharp
public class ApiTimeoutException : ApiConnectionException, ISerializable
```


The full response did not arrive within the timeout. A kind of [ApiConnectionException](/api/TypeSafe.AI.Sdk/ApiConnectionException/).
Mirrors JS `APITimeoutError`.

**Inheritance:** [object](https://learn.microsoft.com/dotnet/api/system.object) → [Exception](https://learn.microsoft.com/dotnet/api/system.exception) → [TypeSafeException](/api/TypeSafe.AI.Sdk/TypeSafeException/) → [ApiConnectionException](/api/TypeSafe.AI.Sdk/ApiConnectionException/)

**Implements:** [ISerializable](https://learn.microsoft.com/dotnet/api/system.runtime.serialization.iserializable)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Errors/TypeSafeExceptions.cs#L287)

## Constructors

<a id="TypeSafe_AI_Sdk_ApiTimeoutException__ctor_"></a>
<a id="TypeSafe_AI_Sdk_ApiTimeoutException__ctor_System_Int32_System_Exception_"></a>

### ApiTimeoutException(int, Exception?)


```csharp
public ApiTimeoutException(int timeoutMs, Exception? innerException = null)
```


Create a timeout error.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `timeoutMs` | [int](https://learn.microsoft.com/dotnet/api/system.int32) |  |
| `innerException` | [Exception](https://learn.microsoft.com/dotnet/api/system.exception) |  |

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Errors/TypeSafeExceptions.cs#L293)

## Properties

<a id="TypeSafe_AI_Sdk_ApiTimeoutException_TimeoutMs_"></a>
<a id="TypeSafe_AI_Sdk_ApiTimeoutException_TimeoutMs"></a>

### TimeoutMs


```csharp
public int TimeoutMs { get; }
```


Configured timeout in milliseconds.

**Value:** [int](https://learn.microsoft.com/dotnet/api/system.int32)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Errors/TypeSafeExceptions.cs#L290)
