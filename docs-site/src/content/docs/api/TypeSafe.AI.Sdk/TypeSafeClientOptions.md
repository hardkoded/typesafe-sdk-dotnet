---
title: "TypeSafeClientOptions"
description: "Client options. Explicit values take precedence over environment variables, then SDK defaults."
editUrl: false
prev: false
next: false
sidebar:
  label: "TypeSafeClientOptions"
  hidden: true
---

<a id="TypeSafe_AI_Sdk_TypeSafeClientOptions"></a>

Namespace: [TypeSafe.AI.Sdk](/api/TypeSafe.AI.Sdk/)


```csharp
public sealed class TypeSafeClientOptions
```


Client options. Explicit values take precedence over environment variables, then SDK defaults.

**Inheritance:** [object](https://learn.microsoft.com/dotnet/api/system.object)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Requests/SystemOneRequest.cs#L50)

## Properties

<a id="TypeSafe_AI_Sdk_TypeSafeClientOptions_ApiKey_"></a>
<a id="TypeSafe_AI_Sdk_TypeSafeClientOptions_ApiKey"></a>

### ApiKey


```csharp
public string? ApiKey { get; set; }
```


Required API key; falls back to `TYPESAFE_API_KEY`.

**Value:** [string](https://learn.microsoft.com/dotnet/api/system.string)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Requests/SystemOneRequest.cs#L53)

<a id="TypeSafe_AI_Sdk_TypeSafeClientOptions_BaseUrl_"></a>
<a id="TypeSafe_AI_Sdk_TypeSafeClientOptions_BaseUrl"></a>

### BaseUrl


```csharp
public string? BaseUrl { get; set; }
```


API root; falls back to `TYPESAFE_BASE_URL`, then `https://api.typesafe.ai`.

**Value:** [string](https://learn.microsoft.com/dotnet/api/system.string)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Requests/SystemOneRequest.cs#L56)

<a id="TypeSafe_AI_Sdk_TypeSafeClientOptions_Compression_"></a>
<a id="TypeSafe_AI_Sdk_TypeSafeClientOptions_Compression"></a>

### Compression


```csharp
public bool Compression { get; set; }
```


Compress request bodies of 1 KiB or more with zstd. Default: `true`.
Set to `false` to send all bodies uncompressed, for example to an API that doesn't accept compressed requests.

**Value:** [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Requests/SystemOneRequest.cs#L84)

<a id="TypeSafe_AI_Sdk_TypeSafeClientOptions_DefaultHeaders_"></a>
<a id="TypeSafe_AI_Sdk_TypeSafeClientOptions_DefaultHeaders"></a>

### DefaultHeaders


```csharp
public IDictionary<string, string>? DefaultHeaders { get; set; }
```


Additional request headers; per-call headers take precedence.

**Value:** [IDictionary&lt;string, string&gt;](https://learn.microsoft.com/dotnet/api/system.collections.generic.idictionary-2)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Requests/SystemOneRequest.cs#L87)

<a id="TypeSafe_AI_Sdk_TypeSafeClientOptions_DefaultModel_"></a>
<a id="TypeSafe_AI_Sdk_TypeSafeClientOptions_DefaultModel"></a>

### DefaultModel


```csharp
public string? DefaultModel { get; set; }
```


Default model; falls back to `TYPESAFE_DEFAULT_MODEL`, then `jev-latest`.

**Value:** [string](https://learn.microsoft.com/dotnet/api/system.string)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Requests/SystemOneRequest.cs#L59)

<a id="TypeSafe_AI_Sdk_TypeSafeClientOptions_HttpMessageHandler_"></a>
<a id="TypeSafe_AI_Sdk_TypeSafeClientOptions_HttpMessageHandler"></a>

### HttpMessageHandler


```csharp
public HttpMessageHandler? HttpMessageHandler { get; set; }
```


Optional [HttpMessageHandler](#TypeSafe_AI_Sdk_TypeSafeClientOptions_HttpMessageHandler) used when the client creates its own [HttpClient](https://learn.microsoft.com/dotnet/api/system.net.http.httpclient).
Useful for tests.

**Value:** [HttpMessageHandler](https://learn.microsoft.com/dotnet/api/system.net.http.httpmessagehandler)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Requests/SystemOneRequest.cs#L93)

<a id="TypeSafe_AI_Sdk_TypeSafeClientOptions_LogLevel_"></a>
<a id="TypeSafe_AI_Sdk_TypeSafeClientOptions_LogLevel"></a>

### LogLevel


```csharp
public TypeSafeLogLevel? LogLevel { get; set; }
```


Log level; falls back to `TYPESAFE_LOG_LEVEL`, then `warn`.
`info` logs request summaries; `debug` adds headers and bodies.
Known credential headers are redacted; bodies are not.

**Value:** `TypeSafeLogLevel?`

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Requests/SystemOneRequest.cs#L66)

<a id="TypeSafe_AI_Sdk_TypeSafeClientOptions_LogLevelText_"></a>
<a id="TypeSafe_AI_Sdk_TypeSafeClientOptions_LogLevelText"></a>

### LogLevelText


```csharp
public string? LogLevelText { get; set; }
```


Raw log-level string (for env-style values such as `off`).

**Value:** [string](https://learn.microsoft.com/dotnet/api/system.string)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Requests/SystemOneRequest.cs#L69)

<a id="TypeSafe_AI_Sdk_TypeSafeClientOptions_Logger_"></a>
<a id="TypeSafe_AI_Sdk_TypeSafeClientOptions_Logger"></a>

### Logger


```csharp
public ITypeSafeLogger? Logger { get; set; }
```


Logger filtered to [LogLevel](#TypeSafe_AI_Sdk_TypeSafeClientOptions_LogLevel) and above. Default: prefixed console.

**Value:** [ITypeSafeLogger](/api/TypeSafe.AI.Sdk/ITypeSafeLogger/)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Requests/SystemOneRequest.cs#L72)

<a id="TypeSafe_AI_Sdk_TypeSafeClientOptions_Retry_"></a>
<a id="TypeSafe_AI_Sdk_TypeSafeClientOptions_Retry"></a>

### Retry


```csharp
public RetryPolicy? Retry { get; set; }
```


Retry overrides; omitted fields use the defaults in [RetryPolicy](/api/TypeSafe.AI.Sdk/RetryPolicy/).

**Value:** [RetryPolicy](/api/TypeSafe.AI.Sdk/RetryPolicy/)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Requests/SystemOneRequest.cs#L75)

<a id="TypeSafe_AI_Sdk_TypeSafeClientOptions_TimeoutMs_"></a>
<a id="TypeSafe_AI_Sdk_TypeSafeClientOptions_TimeoutMs"></a>

### TimeoutMs


```csharp
public int? TimeoutMs { get; set; }
```


Timeout per attempt in milliseconds, without a total retry budget. Default: 10000.

**Value:** [int?](https://learn.microsoft.com/dotnet/api/system.int32)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Requests/SystemOneRequest.cs#L78)
