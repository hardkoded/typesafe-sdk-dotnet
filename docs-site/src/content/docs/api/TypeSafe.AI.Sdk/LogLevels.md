---
title: "LogLevels"
description: "Supported log level names, from most to least verbose."
editUrl: false
prev: false
next: false
sidebar:
  label: "LogLevels"
  hidden: true
---

<a id="TypeSafe_AI_Sdk_LogLevels"></a>

Namespace: [TypeSafe.AI.Sdk](/api/TypeSafe.AI.Sdk/)


```csharp
public static class LogLevels
```


Supported log level names, from most to least verbose.

**Inheritance:** [object](https://learn.microsoft.com/dotnet/api/system.object)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Logging/Logging.cs#L26)

## Fields

<a id="TypeSafe_AI_Sdk_LogLevels_All"></a>

### All


```csharp
public static readonly IReadOnlyList<string> All
```


Canonical names matching the JS SDK: debug, info, warn, error, off.

**Value:** [IReadOnlyList&lt;string&gt;](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlylist-1)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Logging/Logging.cs#L29)

<a id="TypeSafe_AI_Sdk_LogLevels_Default"></a>

### Default


```csharp
public const TypeSafeLogLevel Default = Warn
```


Default log level (`warn`).

**Value:** [TypeSafeLogLevel](/api/TypeSafe.AI.Sdk/TypeSafeLogLevel/)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Logging/Logging.cs#L32)

## Methods

<a id="TypeSafe_AI_Sdk_LogLevels_Parse_"></a>
<a id="TypeSafe_AI_Sdk_LogLevels_Parse_System_String_System_String_"></a>

### Parse(string, string)


```csharp
public static TypeSafeLogLevel Parse(string value, string source)
```


Validate a configured log level, throwing [TypeSafeException](/api/TypeSafe.AI.Sdk/TypeSafeException/) for unknown values.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `value` | [string](https://learn.microsoft.com/dotnet/api/system.string) |  |
| `source` | [string](https://learn.microsoft.com/dotnet/api/system.string) |  |

**Returns:** [TypeSafeLogLevel](/api/TypeSafe.AI.Sdk/TypeSafeLogLevel/)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Logging/Logging.cs#L35)

<a id="TypeSafe_AI_Sdk_LogLevels_ToJsName_"></a>
<a id="TypeSafe_AI_Sdk_LogLevels_ToJsName_TypeSafe_AI_Sdk_TypeSafeLogLevel_"></a>

### ToJsName(TypeSafeLogLevel)


```csharp
public static string ToJsName(TypeSafeLogLevel level)
```


JS-style name for a level.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `level` | [TypeSafeLogLevel](/api/TypeSafe.AI.Sdk/TypeSafeLogLevel/) |  |

**Returns:** [string](https://learn.microsoft.com/dotnet/api/system.string)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Logging/Logging.cs#L53)
