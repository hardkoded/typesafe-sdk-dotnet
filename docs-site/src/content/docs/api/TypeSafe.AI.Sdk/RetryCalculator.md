---
title: "RetryCalculator"
description: "Retry delay calculation and cancellable waits. Mirrors JS retry.ts."
editUrl: false
prev: false
next: false
sidebar:
  label: "RetryCalculator"
  hidden: true
---

<a id="TypeSafe_AI_Sdk_RetryCalculator"></a>

Namespace: [TypeSafe.AI.Sdk](/api/TypeSafe.AI.Sdk/)


```csharp
public static class RetryCalculator
```


Retry delay calculation and cancellable waits. Mirrors JS `retry.ts`.

**Inheritance:** [object](https://learn.microsoft.com/dotnet/api/system.object)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Retry/RetryCalculator.cs#L10)

## Fields

<a id="TypeSafe_AI_Sdk_RetryCalculator_DefaultTimeoutMs"></a>

### DefaultTimeoutMs


```csharp
public const int DefaultTimeoutMs = 10000
```


Default timeout per attempt in milliseconds.

**Value:** [int](https://learn.microsoft.com/dotnet/api/system.int32)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Retry/RetryCalculator.cs#L13)

## Methods

<a id="TypeSafe_AI_Sdk_RetryCalculator_DelayMs_"></a>
<a id="TypeSafe_AI_Sdk_RetryCalculator_DelayMs_System_Int32_System_Net_Http_Headers_HttpResponseHeaders_TypeSafe_AI_Sdk_RetryPolicy_System_Func_System_Double__"></a>

### `DelayMs(int, HttpResponseHeaders?, RetryPolicy?, Func<double>?)`


```csharp
public static int DelayMs(int attempt, HttpResponseHeaders? headers = null, RetryPolicy? policy = null, Func<double>? random = null)
```


Calculate the delay in milliseconds for a zero-based retry attempt.
Uses an allowed server delay; otherwise uses capped exponential backoff with jitter.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `attempt` | [int](https://learn.microsoft.com/dotnet/api/system.int32) |  |
| `headers` | [HttpResponseHeaders](https://learn.microsoft.com/dotnet/api/system.net.http.headers.httpresponseheaders) |  |
| `policy` | [RetryPolicy](/api/TypeSafe.AI.Sdk/RetryPolicy/) |  |
| `random` | [Func&lt;double&gt;](https://learn.microsoft.com/dotnet/api/system.func-1) |  |

**Returns:** [int](https://learn.microsoft.com/dotnet/api/system.int32)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Retry/RetryCalculator.cs#L59)

<a id="TypeSafe_AI_Sdk_RetryCalculator_IsRetryableStatus_"></a>
<a id="TypeSafe_AI_Sdk_RetryCalculator_IsRetryableStatus_System_Int32_TypeSafe_AI_Sdk_RetryPolicy_"></a>

### IsRetryableStatus(int, RetryPolicy?)


```csharp
public static bool IsRetryableStatus(int status, RetryPolicy? policy = null)
```


Whether the policy retries an HTTP status code.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `status` | [int](https://learn.microsoft.com/dotnet/api/system.int32) |  |
| `policy` | [RetryPolicy](/api/TypeSafe.AI.Sdk/RetryPolicy/) |  |

**Returns:** [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Retry/RetryCalculator.cs#L16)

<a id="TypeSafe_AI_Sdk_RetryCalculator_ParseRetryAfter_"></a>
<a id="TypeSafe_AI_Sdk_RetryCalculator_ParseRetryAfter_System_Net_Http_Headers_HttpResponseHeaders_System_Nullable_System_DateTimeOffset__"></a>

### ParseRetryAfter(HttpResponseHeaders, DateTimeOffset?)


```csharp
public static int? ParseRetryAfter(HttpResponseHeaders headers, DateTimeOffset? now = null)
```


Parse `retry-after-ms` or `Retry-After` into milliseconds, preferring `retry-after-ms`.
Returns `null` when neither header contains a valid delay.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `headers` | [HttpResponseHeaders](https://learn.microsoft.com/dotnet/api/system.net.http.headers.httpresponseheaders) |  |
| `now` | [DateTimeOffset?](https://learn.microsoft.com/dotnet/api/system.datetimeoffset) |  |

**Returns:** [int?](https://learn.microsoft.com/dotnet/api/system.int32)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Retry/RetryCalculator.cs#L23)

<a id="TypeSafe_AI_Sdk_RetryCalculator_SleepAsync_"></a>
<a id="TypeSafe_AI_Sdk_RetryCalculator_SleepAsync_System_Int32_System_Threading_CancellationToken_"></a>

### SleepAsync(int, CancellationToken)


```csharp
public static Task SleepAsync(int milliseconds, CancellationToken cancellationToken = default)
```


Wait &lt;code class="paramref"&gt;milliseconds&lt;/code&gt;, throwing [OperationCanceledException](https://learn.microsoft.com/dotnet/api/system.operationcanceledexception) on cancellation.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `milliseconds` | [int](https://learn.microsoft.com/dotnet/api/system.int32) |  |
| `cancellationToken` | [CancellationToken](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) |  |

**Returns:** [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Retry/RetryCalculator.cs#L81)
