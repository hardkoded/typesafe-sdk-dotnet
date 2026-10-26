---
title: "ApiUserAbortException"
description: "The caller cancelled the request through a CancellationToken. Mirrors JS APIUserAbortError."
editUrl: false
prev: false
next: false
sidebar:
  label: "ApiUserAbortException"
  hidden: true
---

<a id="TypeSafe_AI_Sdk_ApiUserAbortException"></a>

Namespace: [TypeSafe.AI.Sdk](/api/TypeSafe.AI.Sdk/)


```csharp
public class ApiUserAbortException : TypeSafeException, ISerializable
```


The caller cancelled the request through a [CancellationToken](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken).
Mirrors JS `APIUserAbortError`.

**Inheritance:** [object](https://learn.microsoft.com/dotnet/api/system.object) → [Exception](https://learn.microsoft.com/dotnet/api/system.exception) → [TypeSafeException](/api/TypeSafe.AI.Sdk/TypeSafeException/)

**Implements:** [ISerializable](https://learn.microsoft.com/dotnet/api/system.runtime.serialization.iserializable)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Errors/TypeSafeExceptions.cs#L304)

## Constructors

<a id="TypeSafe_AI_Sdk_ApiUserAbortException__ctor_"></a>
<a id="TypeSafe_AI_Sdk_ApiUserAbortException__ctor_System_String_System_Exception_"></a>

### ApiUserAbortException(string?, Exception?)


```csharp
public ApiUserAbortException(string? message = null, Exception? innerException = null)
```


Create an abort error.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `message` | [string](https://learn.microsoft.com/dotnet/api/system.string) |  |
| `innerException` | [Exception](https://learn.microsoft.com/dotnet/api/system.exception) |  |

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Errors/TypeSafeExceptions.cs#L307)
