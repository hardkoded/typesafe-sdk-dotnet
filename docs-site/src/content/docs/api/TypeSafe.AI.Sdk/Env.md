---
title: "Env"
description: "Environment variable names used when constructing TypeSafeClient. Explicit constructor options take precedence over these values. Empty or whitespace-only va..."
editUrl: false
prev: false
next: false
sidebar:
  label: "Env"
  hidden: true
---

<a id="TypeSafe_AI_Sdk_Env"></a>

Namespace: [TypeSafe.AI.Sdk](/api/TypeSafe.AI.Sdk/)


```csharp
public static class Env
```


Environment variable names used when constructing [TypeSafeClient](/api/TypeSafe.AI.Sdk/TypeSafeClient/).
Explicit constructor options take precedence over these values.
Empty or whitespace-only values are ignored.

**Inheritance:** [object](https://learn.microsoft.com/dotnet/api/system.object)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Env.cs#L11)

## Fields

<a id="TypeSafe_AI_Sdk_Env_ApiKey"></a>

### ApiKey


```csharp
public const string ApiKey = "TYPESAFE_API_KEY"
```


Required API key; used when [ApiKey](/api/TypeSafe.AI.Sdk/TypeSafeClientOptions/#TypeSafe_AI_Sdk_TypeSafeClientOptions_ApiKey) is omitted.

**Value:** [string](https://learn.microsoft.com/dotnet/api/system.string)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Env.cs#L14)

<a id="TypeSafe_AI_Sdk_Env_BaseUrl"></a>

### BaseUrl


```csharp
public const string BaseUrl = "TYPESAFE_BASE_URL"
```


API root; defaults to `https://api.typesafe.ai`.

**Value:** [string](https://learn.microsoft.com/dotnet/api/system.string)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Env.cs#L17)

<a id="TypeSafe_AI_Sdk_Env_DefaultModel"></a>

### DefaultModel


```csharp
public const string DefaultModel = "TYPESAFE_DEFAULT_MODEL"
```


Default model name; defaults to `jev-latest`.

**Value:** [string](https://learn.microsoft.com/dotnet/api/system.string)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Env.cs#L20)

<a id="TypeSafe_AI_Sdk_Env_LogLevel"></a>

### LogLevel


```csharp
public const string LogLevel = "TYPESAFE_LOG_LEVEL"
```


Log level; defaults to `warn`.

**Value:** [string](https://learn.microsoft.com/dotnet/api/system.string)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Env.cs#L23)

## Methods

<a id="TypeSafe_AI_Sdk_Env_FromCodeOrEnv_"></a>
<a id="TypeSafe_AI_Sdk_Env_FromCodeOrEnv_System_String_System_String_"></a>

### FromCodeOrEnv(string?, string)


```csharp
public static string? FromCodeOrEnv(string? fromCode, string envVar)
```


Return the explicit value, falling back to the environment.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `fromCode` | [string](https://learn.microsoft.com/dotnet/api/system.string) |  |
| `envVar` | [string](https://learn.microsoft.com/dotnet/api/system.string) |  |

**Returns:** [string](https://learn.microsoft.com/dotnet/api/system.string)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Env.cs#L38)

<a id="TypeSafe_AI_Sdk_Env_Read_"></a>
<a id="TypeSafe_AI_Sdk_Env_Read_System_String_"></a>

### Read(string)


```csharp
public static string? Read(string name)
```


Read a trimmed environment value, returning `null` for missing or blank values.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `name` | [string](https://learn.microsoft.com/dotnet/api/system.string) |  |

**Returns:** [string](https://learn.microsoft.com/dotnet/api/system.string)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Env.cs#L26)
