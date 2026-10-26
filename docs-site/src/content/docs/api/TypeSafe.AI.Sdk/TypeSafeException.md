---
title: "TypeSafeException"
description: "Base class for SDK errors. Mirrors JS TypeSafeError."
editUrl: false
prev: false
next: false
sidebar:
  label: "TypeSafeException"
  hidden: true
---

<a id="TypeSafe_AI_Sdk_TypeSafeException"></a>

Namespace: [TypeSafe.AI.Sdk](/api/TypeSafe.AI.Sdk/)


```csharp
public class TypeSafeException : Exception, ISerializable
```


Base class for SDK errors. Mirrors JS `TypeSafeError`.

**Inheritance:** [object](https://learn.microsoft.com/dotnet/api/system.object) → [Exception](https://learn.microsoft.com/dotnet/api/system.exception)

**Implements:** [ISerializable](https://learn.microsoft.com/dotnet/api/system.runtime.serialization.iserializable)

**Derived:** [ApiConnectionException](/api/TypeSafe.AI.Sdk/ApiConnectionException/), [ApiException](/api/TypeSafe.AI.Sdk/ApiException/), [ApiUserAbortException](/api/TypeSafe.AI.Sdk/ApiUserAbortException/)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Errors/TypeSafeExceptions.cs#L11)

## Constructors

<a id="TypeSafe_AI_Sdk_TypeSafeException__ctor_"></a>
<a id="TypeSafe_AI_Sdk_TypeSafeException__ctor_System_String_System_Exception_"></a>

### TypeSafeException(string, Exception?)


```csharp
public TypeSafeException(string message, Exception? innerException = null)
```


Create an SDK error.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `message` | [string](https://learn.microsoft.com/dotnet/api/system.string) |  |
| `innerException` | [Exception](https://learn.microsoft.com/dotnet/api/system.exception) |  |

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Errors/TypeSafeExceptions.cs#L14)
