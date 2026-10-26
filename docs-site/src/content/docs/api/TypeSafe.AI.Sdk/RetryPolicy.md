---
title: "RetryPolicy"
description: "Retry configuration. Partial overrides inherit unset fields from the client or SDK defaults. Defaults match the JavaScript SDK."
editUrl: false
prev: false
next: false
sidebar:
  label: "RetryPolicy"
  hidden: true
---

<a id="TypeSafe_AI_Sdk_RetryPolicy"></a>

Namespace: [TypeSafe.AI.Sdk](/api/TypeSafe.AI.Sdk/)


```csharp
public sealed class RetryPolicy
```


Retry configuration. Partial overrides inherit unset fields from the client or SDK defaults.
Defaults match the JavaScript SDK.

**Inheritance:** [object](https://learn.microsoft.com/dotnet/api/system.object)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Retry/RetryPolicy.cs#L10)

## Properties

<a id="TypeSafe_AI_Sdk_RetryPolicy_ApiConnectionError_"></a>
<a id="TypeSafe_AI_Sdk_RetryPolicy_ApiConnectionError"></a>

### ApiConnectionError


```csharp
public bool ApiConnectionError { get; set; }
```


Retry connection failures, including interrupted response bodies. Default: true.

**Value:** [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Retry/RetryPolicy.cs#L100)

<a id="TypeSafe_AI_Sdk_RetryPolicy_ApiTimeoutError_"></a>
<a id="TypeSafe_AI_Sdk_RetryPolicy_ApiTimeoutError"></a>

### ApiTimeoutError


```csharp
public bool ApiTimeoutError { get; set; }
```


Whether to retry [ApiTimeoutException](/api/TypeSafe.AI.Sdk/ApiTimeoutException/). Default: true.

**Value:** [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Retry/RetryPolicy.cs#L111)

<a id="TypeSafe_AI_Sdk_RetryPolicy_BackoffInitialMs_"></a>
<a id="TypeSafe_AI_Sdk_RetryPolicy_BackoffInitialMs"></a>

### BackoffInitialMs


```csharp
public int BackoffInitialMs { get; set; }
```


First backoff delay in milliseconds, doubled up to [BackoffMaxMs](#TypeSafe_AI_Sdk_RetryPolicy_BackoffMaxMs). Default: 500.

**Value:** [int](https://learn.microsoft.com/dotnet/api/system.int32)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Retry/RetryPolicy.cs#L34)

<a id="TypeSafe_AI_Sdk_RetryPolicy_BackoffJitter_"></a>
<a id="TypeSafe_AI_Sdk_RetryPolicy_BackoffJitter"></a>

### BackoffJitter


```csharp
public double BackoffJitter { get; set; }
```


Fraction of each backoff delay randomly subtracted, from 0 to 1. Default: 0.25.

**Value:** [double](https://learn.microsoft.com/dotnet/api/system.double)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Retry/RetryPolicy.cs#L56)

<a id="TypeSafe_AI_Sdk_RetryPolicy_BackoffMaxMs_"></a>
<a id="TypeSafe_AI_Sdk_RetryPolicy_BackoffMaxMs"></a>

### BackoffMaxMs


```csharp
public int BackoffMaxMs { get; set; }
```


Maximum backoff delay in milliseconds. Default: 5000.

**Value:** [int](https://learn.microsoft.com/dotnet/api/system.int32)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Retry/RetryPolicy.cs#L45)

<a id="TypeSafe_AI_Sdk_RetryPolicy_Default_"></a>
<a id="TypeSafe_AI_Sdk_RetryPolicy_Default"></a>

### Default


```csharp
public static RetryPolicy Default { get; }
```


SDK default retry policy (a new isolated copy).

**Value:** [RetryPolicy](./)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Retry/RetryPolicy.cs#L122)

<a id="TypeSafe_AI_Sdk_RetryPolicy_HttpStatuses_"></a>
<a id="TypeSafe_AI_Sdk_RetryPolicy_HttpStatuses"></a>

### HttpStatuses


```csharp
public ISet<int> HttpStatuses { get; set; }
```


HTTP status codes to retry. Default: 408, 429, and 500–599.

**Value:** [ISet&lt;int&gt;](https://learn.microsoft.com/dotnet/api/system.collections.generic.iset-1)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Retry/RetryPolicy.cs#L67)

<a id="TypeSafe_AI_Sdk_RetryPolicy_MaxRetries_"></a>
<a id="TypeSafe_AI_Sdk_RetryPolicy_MaxRetries"></a>

### MaxRetries


```csharp
public int MaxRetries { get; set; }
```


Maximum retries after the initial attempt; `0` disables retries. Default: 2.

**Value:** [int](https://learn.microsoft.com/dotnet/api/system.int32)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Retry/RetryPolicy.cs#L23)

<a id="TypeSafe_AI_Sdk_RetryPolicy_MaxRetryAfterMs_"></a>
<a id="TypeSafe_AI_Sdk_RetryPolicy_MaxRetryAfterMs"></a>

### MaxRetryAfterMs


```csharp
public int MaxRetryAfterMs { get; set; }
```


Maximum server retry delay in milliseconds; longer delays use backoff. Default: 60000.

**Value:** [int](https://learn.microsoft.com/dotnet/api/system.int32)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Retry/RetryPolicy.cs#L89)

<a id="TypeSafe_AI_Sdk_RetryPolicy_RespectRetryAfter_"></a>
<a id="TypeSafe_AI_Sdk_RetryPolicy_RespectRetryAfter"></a>

### RespectRetryAfter


```csharp
public bool RespectRetryAfter { get; set; }
```


Honor `Retry-After` and `retry-after-ms` up to [MaxRetryAfterMs](#TypeSafe_AI_Sdk_RetryPolicy_MaxRetryAfterMs). Default: true.

**Value:** [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Retry/RetryPolicy.cs#L78)

## Methods

<a id="TypeSafe_AI_Sdk_RetryPolicy_Clone_"></a>
<a id="TypeSafe_AI_Sdk_RetryPolicy_Clone"></a>

### Clone()


```csharp
public RetryPolicy Clone()
```


Copy this policy so later mutations cannot affect in-flight requests.

**Returns:** [RetryPolicy](./)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Retry/RetryPolicy.cs#L125)

<a id="TypeSafe_AI_Sdk_RetryPolicy_Resolve_"></a>
<a id="TypeSafe_AI_Sdk_RetryPolicy_Resolve_TypeSafe_AI_Sdk_RetryPolicy_TypeSafe_AI_Sdk_RetryPolicy_"></a>

### Resolve(RetryPolicy, RetryPolicy?)


```csharp
public static RetryPolicy Resolve(RetryPolicy baseline, RetryPolicy? overrides)
```


Merge overrides onto a base policy and validate every numeric field.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `baseline` | [RetryPolicy](./) |  |
| `overrides` | [RetryPolicy](./) |  |

**Returns:** [RetryPolicy](./)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Retry/RetryPolicy.cs#L142)

<a id="TypeSafe_AI_Sdk_RetryPolicy_Validate_"></a>
<a id="TypeSafe_AI_Sdk_RetryPolicy_Validate"></a>

### Validate()


```csharp
public RetryPolicy Validate()
```


Validate a fully-specified constructor policy.

**Returns:** [RetryPolicy](./)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Retry/RetryPolicy.cs#L200)
