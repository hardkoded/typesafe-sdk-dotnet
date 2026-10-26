---
title: "ConsoleTypeSafeLogger"
description: "Default console logger with the [typesafe-sdk] prefix."
editUrl: false
prev: false
next: false
sidebar:
  label: "ConsoleTypeSafeLogger"
  hidden: true
---

<a id="TypeSafe_AI_Sdk_ConsoleTypeSafeLogger"></a>

Namespace: [TypeSafe.AI.Sdk](/api/TypeSafe.AI.Sdk/)


```csharp
public sealed class ConsoleTypeSafeLogger : ITypeSafeLogger
```


Default console logger with the `[typesafe-sdk]` prefix.

**Inheritance:** [object](https://learn.microsoft.com/dotnet/api/system.object)

**Implements:** [ITypeSafeLogger](/api/TypeSafe.AI.Sdk/ITypeSafeLogger/)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Logging/Logging.cs#L82)

## Methods

<a id="TypeSafe_AI_Sdk_ConsoleTypeSafeLogger_Debug_"></a>
<a id="TypeSafe_AI_Sdk_ConsoleTypeSafeLogger_Debug_System_String_System_Object___"></a>

### Debug(string, params object?[])


```csharp
public void Debug(string message, params object?[] args)
```


Write a debug message.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `message` | [string](https://learn.microsoft.com/dotnet/api/system.string) |  |
| `args` | [object\[\]](https://learn.microsoft.com/dotnet/api/system.object) |  |

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Logging/Logging.cs#L87)

<a id="TypeSafe_AI_Sdk_ConsoleTypeSafeLogger_Error_"></a>
<a id="TypeSafe_AI_Sdk_ConsoleTypeSafeLogger_Error_System_String_System_Object___"></a>

### Error(string, params object?[])


```csharp
public void Error(string message, params object?[] args)
```


Write an error message.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `message` | [string](https://learn.microsoft.com/dotnet/api/system.string) |  |
| `args` | [object\[\]](https://learn.microsoft.com/dotnet/api/system.object) |  |

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Logging/Logging.cs#L99)

<a id="TypeSafe_AI_Sdk_ConsoleTypeSafeLogger_Info_"></a>
<a id="TypeSafe_AI_Sdk_ConsoleTypeSafeLogger_Info_System_String_System_Object___"></a>

### Info(string, params object?[])


```csharp
public void Info(string message, params object?[] args)
```


Write an informational message.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `message` | [string](https://learn.microsoft.com/dotnet/api/system.string) |  |
| `args` | [object\[\]](https://learn.microsoft.com/dotnet/api/system.object) |  |

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Logging/Logging.cs#L91)

<a id="TypeSafe_AI_Sdk_ConsoleTypeSafeLogger_Warn_"></a>
<a id="TypeSafe_AI_Sdk_ConsoleTypeSafeLogger_Warn_System_String_System_Object___"></a>

### Warn(string, params object?[])


```csharp
public void Warn(string message, params object?[] args)
```


Write a warning.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `message` | [string](https://learn.microsoft.com/dotnet/api/system.string) |  |
| `args` | [object\[\]](https://learn.microsoft.com/dotnet/api/system.object) |  |

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Logging/Logging.cs#L95)
