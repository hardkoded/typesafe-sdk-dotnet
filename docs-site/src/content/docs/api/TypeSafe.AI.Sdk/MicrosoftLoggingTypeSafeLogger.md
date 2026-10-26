---
title: "MicrosoftLoggingTypeSafeLogger"
description: "Adapts ILogger to ITypeSafeLogger."
editUrl: false
prev: false
next: false
sidebar:
  label: "MicrosoftLoggingTypeSafeLogger"
  hidden: true
---

<a id="TypeSafe_AI_Sdk_MicrosoftLoggingTypeSafeLogger"></a>

Namespace: [TypeSafe.AI.Sdk](/api/TypeSafe.AI.Sdk/)


```csharp
public sealed class MicrosoftLoggingTypeSafeLogger : ITypeSafeLogger
```


Adapts [ILogger](https://learn.microsoft.com/dotnet/api/microsoft.extensions.logging.ilogger) to [ITypeSafeLogger](/api/TypeSafe.AI.Sdk/ITypeSafeLogger/).

**Inheritance:** [object](https://learn.microsoft.com/dotnet/api/system.object)

**Implements:** [ITypeSafeLogger](/api/TypeSafe.AI.Sdk/ITypeSafeLogger/)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Logging/Logging.cs#L121)

## Constructors

<a id="TypeSafe_AI_Sdk_MicrosoftLoggingTypeSafeLogger__ctor_"></a>
<a id="TypeSafe_AI_Sdk_MicrosoftLoggingTypeSafeLogger__ctor_Microsoft_Extensions_Logging_ILogger_"></a>

### MicrosoftLoggingTypeSafeLogger(ILogger)


```csharp
public MicrosoftLoggingTypeSafeLogger(ILogger logger)
```


Create an adapter around a Microsoft logger.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `logger` | [ILogger](https://learn.microsoft.com/dotnet/api/microsoft.extensions.logging.ilogger) |  |

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Logging/Logging.cs#L126)

## Methods

<a id="TypeSafe_AI_Sdk_MicrosoftLoggingTypeSafeLogger_Debug_"></a>
<a id="TypeSafe_AI_Sdk_MicrosoftLoggingTypeSafeLogger_Debug_System_String_System_Object___"></a>

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

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Logging/Logging.cs#L137)

<a id="TypeSafe_AI_Sdk_MicrosoftLoggingTypeSafeLogger_Error_"></a>
<a id="TypeSafe_AI_Sdk_MicrosoftLoggingTypeSafeLogger_Error_System_String_System_Object___"></a>

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

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Logging/Logging.cs#L149)

<a id="TypeSafe_AI_Sdk_MicrosoftLoggingTypeSafeLogger_Info_"></a>
<a id="TypeSafe_AI_Sdk_MicrosoftLoggingTypeSafeLogger_Info_System_String_System_Object___"></a>

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

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Logging/Logging.cs#L141)

<a id="TypeSafe_AI_Sdk_MicrosoftLoggingTypeSafeLogger_Warn_"></a>
<a id="TypeSafe_AI_Sdk_MicrosoftLoggingTypeSafeLogger_Warn_System_String_System_Object___"></a>

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

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Logging/Logging.cs#L145)
