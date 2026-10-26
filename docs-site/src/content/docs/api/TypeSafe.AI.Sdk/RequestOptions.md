---
title: "RequestOptions"
description: "Per-call options that override client settings."
editUrl: false
prev: false
next: false
sidebar:
  label: "RequestOptions"
  hidden: true
---

<a id="TypeSafe_AI_Sdk_RequestOptions"></a>

Namespace: [TypeSafe.AI.Sdk](/api/TypeSafe.AI.Sdk/)


```csharp
public sealed class RequestOptions
```


Per-call options that override client settings.

**Inheritance:** [object](https://learn.microsoft.com/dotnet/api/system.object)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Requests/SystemOneRequest.cs#L34)

## Properties

<a id="TypeSafe_AI_Sdk_RequestOptions_CancellationToken_"></a>
<a id="TypeSafe_AI_Sdk_RequestOptions_CancellationToken"></a>

### CancellationToken


```csharp
public CancellationToken CancellationToken { get; set; }
```


Cancellation token for the request and pending retries.

**Value:** [CancellationToken](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Requests/SystemOneRequest.cs#L46)

<a id="TypeSafe_AI_Sdk_RequestOptions_Headers_"></a>
<a id="TypeSafe_AI_Sdk_RequestOptions_Headers"></a>

### Headers


```csharp
public IDictionary<string, string>? Headers { get; set; }
```


Additional headers, merged over [DefaultHeaders](/api/TypeSafe.AI.Sdk/TypeSafeClientOptions/#TypeSafe_AI_Sdk_TypeSafeClientOptions_DefaultHeaders).

**Value:** [IDictionary&lt;string, string&gt;](https://learn.microsoft.com/dotnet/api/system.collections.generic.idictionary-2)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Requests/SystemOneRequest.cs#L43)

<a id="TypeSafe_AI_Sdk_RequestOptions_Retry_"></a>
<a id="TypeSafe_AI_Sdk_RequestOptions_Retry"></a>

### Retry


```csharp
public RetryPolicy? Retry { get; set; }
```


Retry overrides for this call; omitted fields inherit client settings.

**Value:** [RetryPolicy](/api/TypeSafe.AI.Sdk/RetryPolicy/)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Requests/SystemOneRequest.cs#L40)

<a id="TypeSafe_AI_Sdk_RequestOptions_TimeoutMs_"></a>
<a id="TypeSafe_AI_Sdk_RequestOptions_TimeoutMs"></a>

### TimeoutMs


```csharp
public int? TimeoutMs { get; set; }
```


Timeout per attempt in milliseconds; there is no total retry budget.

**Value:** [int?](https://learn.microsoft.com/dotnet/api/system.int32)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Requests/SystemOneRequest.cs#L37)
