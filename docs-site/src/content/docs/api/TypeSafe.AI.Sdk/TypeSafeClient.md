---
title: "TypeSafeClient"
description: "Client for the TypeSafe AI API. Community .NET port of @typesafe-ai/sdk."
editUrl: false
prev: false
next: false
sidebar:
  label: "TypeSafeClient"
  hidden: true
---

<a id="TypeSafe_AI_Sdk_TypeSafeClient"></a>

Namespace: [TypeSafe.AI.Sdk](/api/TypeSafe.AI.Sdk/)


```csharp
public sealed class TypeSafeClient : ITypeSafeClient, IDisposable
```


Client for the TypeSafe AI API. Community .NET port of `@typesafe-ai/sdk`.

**Inheritance:** [object](https://learn.microsoft.com/dotnet/api/system.object)

**Implements:** [ITypeSafeClient](/api/TypeSafe.AI.Sdk/ITypeSafeClient/), [IDisposable](https://learn.microsoft.com/dotnet/api/system.idisposable)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/TypeSafeClient.cs#L15)

## Constructors

<a id="TypeSafe_AI_Sdk_TypeSafeClient__ctor_"></a>
<a id="TypeSafe_AI_Sdk_TypeSafeClient__ctor_System_Net_Http_HttpClient_TypeSafe_AI_Sdk_TypeSafeClientOptions_"></a>

### TypeSafeClient(HttpClient, TypeSafeClientOptions?)


```csharp
public TypeSafeClient(HttpClient httpClient, TypeSafeClientOptions? options = null)
```


Create a client that uses the given [HttpClient](https://learn.microsoft.com/dotnet/api/system.net.http.httpclient) (typically from `IHttpClientFactory`).

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `httpClient` | [HttpClient](https://learn.microsoft.com/dotnet/api/system.net.http.httpclient) |  |
| `options` | [TypeSafeClientOptions](/api/TypeSafe.AI.Sdk/TypeSafeClientOptions/) |  |

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/TypeSafeClient.cs#L70)

<a id="TypeSafe_AI_Sdk_TypeSafeClient__ctor_System_String_System_Net_Http_HttpClient_TypeSafe_AI_Sdk_TypeSafeClientOptions_"></a>

### TypeSafeClient(string, HttpClient, TypeSafeClientOptions?)


```csharp
public TypeSafeClient(string apiKey, HttpClient httpClient, TypeSafeClientOptions? options = null)
```


Create a client with an explicit API key and [HttpClient](https://learn.microsoft.com/dotnet/api/system.net.http.httpclient).

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `apiKey` | [string](https://learn.microsoft.com/dotnet/api/system.string) |  |
| `httpClient` | [HttpClient](https://learn.microsoft.com/dotnet/api/system.net.http.httpclient) |  |
| `options` | [TypeSafeClientOptions](/api/TypeSafe.AI.Sdk/TypeSafeClientOptions/) |  |

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/TypeSafeClient.cs#L82)

<a id="TypeSafe_AI_Sdk_TypeSafeClient__ctor_System_String_TypeSafe_AI_Sdk_TypeSafeClientOptions_"></a>

### TypeSafeClient(string, TypeSafeClientOptions?)


```csharp
public TypeSafeClient(string apiKey, TypeSafeClientOptions? options = null)
```


Create a client with an explicit API key.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `apiKey` | [string](https://learn.microsoft.com/dotnet/api/system.string) |  |
| `options` | [TypeSafeClientOptions](/api/TypeSafe.AI.Sdk/TypeSafeClientOptions/) |  |

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/TypeSafeClient.cs#L76)

<a id="TypeSafe_AI_Sdk_TypeSafeClient__ctor_TypeSafe_AI_Sdk_TypeSafeClientOptions_"></a>

### TypeSafeClient(TypeSafeClientOptions?)


```csharp
public TypeSafeClient(TypeSafeClientOptions? options = null)
```


Create a client. Explicit options take precedence over environment variables, then SDK defaults.
Empty or whitespace-only environment values are ignored.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `options` | [TypeSafeClientOptions](/api/TypeSafe.AI.Sdk/TypeSafeClientOptions/) |  |

**Exceptions**

| Type | Condition |
| --- | --- |
| [TypeSafeException](/api/TypeSafe.AI.Sdk/TypeSafeException/) | The API key is missing or configuration is invalid. |

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/TypeSafeClient.cs#L64)

## Fields

<a id="TypeSafe_AI_Sdk_TypeSafeClient_DefaultBaseUrl"></a>

### DefaultBaseUrl


```csharp
public const string DefaultBaseUrl = "https://api.typesafe.ai"
```


API root used when neither config nor `TYPESAFE_BASE_URL` is set.

**Value:** [string](https://learn.microsoft.com/dotnet/api/system.string)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/TypeSafeClient.cs#L21)

<a id="TypeSafe_AI_Sdk_TypeSafeClient_DefaultModelName"></a>

### DefaultModelName


```csharp
public const string DefaultModelName = "jev-latest"
```


Model used when a request omits `model`.

**Value:** [string](https://learn.microsoft.com/dotnet/api/system.string)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/TypeSafeClient.cs#L24)

<a id="TypeSafe_AI_Sdk_TypeSafeClient_HttpClientName"></a>

### HttpClientName


```csharp
public const string HttpClientName = "TypeSafe.AI"
```


Named [HttpClient](https://learn.microsoft.com/dotnet/api/system.net.http.httpclient) used with `IHttpClientFactory`.

**Value:** [string](https://learn.microsoft.com/dotnet/api/system.string)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/TypeSafeClient.cs#L18)

## Properties

<a id="TypeSafe_AI_Sdk_TypeSafeClient_BaseUrl_"></a>
<a id="TypeSafe_AI_Sdk_TypeSafeClient_BaseUrl"></a>

### BaseUrl


```csharp
public string BaseUrl { get; }
```


API root with trailing slashes removed.

**Value:** [string](https://learn.microsoft.com/dotnet/api/system.string)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/TypeSafeClient.cs#L33)

<a id="TypeSafe_AI_Sdk_TypeSafeClient_Compression_"></a>
<a id="TypeSafe_AI_Sdk_TypeSafeClient_Compression"></a>

### Compression


```csharp
public bool Compression { get; }
```


Whether request bodies of 1 KiB or more are compressed with zstd.

**Value:** [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/TypeSafeClient.cs#L54)

<a id="TypeSafe_AI_Sdk_TypeSafeClient_DefaultHeaders_"></a>
<a id="TypeSafe_AI_Sdk_TypeSafeClient_DefaultHeaders"></a>

### DefaultHeaders


```csharp
public IReadOnlyDictionary<string, string> DefaultHeaders { get; }
```


Additional headers sent with each request.

**Value:** [IReadOnlyDictionary&lt;string, string&gt;](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlydictionary-2)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/TypeSafeClient.cs#L51)

<a id="TypeSafe_AI_Sdk_TypeSafeClient_DefaultModel_"></a>
<a id="TypeSafe_AI_Sdk_TypeSafeClient_DefaultModel"></a>

### DefaultModel


```csharp
public string DefaultModel { get; }
```


Model used when a request omits `model`.

**Value:** [string](https://learn.microsoft.com/dotnet/api/system.string)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/TypeSafeClient.cs#L36)

<a id="TypeSafe_AI_Sdk_TypeSafeClient_LogLevel_"></a>
<a id="TypeSafe_AI_Sdk_TypeSafeClient_LogLevel"></a>

### LogLevel


```csharp
public TypeSafeLogLevel LogLevel { get; }
```


Configured log verbosity.

**Value:** [TypeSafeLogLevel](/api/TypeSafe.AI.Sdk/TypeSafeLogLevel/)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/TypeSafeClient.cs#L39)

<a id="TypeSafe_AI_Sdk_TypeSafeClient_Logger_"></a>
<a id="TypeSafe_AI_Sdk_TypeSafeClient_Logger"></a>

### Logger


```csharp
public ITypeSafeLogger Logger { get; }
```


The configured logger, filtered to [LogLevel](#TypeSafe_AI_Sdk_TypeSafeClient_LogLevel).

**Value:** [ITypeSafeLogger](/api/TypeSafe.AI.Sdk/ITypeSafeLogger/)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/TypeSafeClient.cs#L42)

<a id="TypeSafe_AI_Sdk_TypeSafeClient_Models_"></a>
<a id="TypeSafe_AI_Sdk_TypeSafeClient_Models"></a>

### Models


```csharp
public ModelsResource Models { get; }
```


The models available to the account.

**Value:** [ModelsResource](/api/TypeSafe.AI.Sdk/ModelsResource/)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/TypeSafeClient.cs#L57)

<a id="TypeSafe_AI_Sdk_TypeSafeClient_Retry_"></a>
<a id="TypeSafe_AI_Sdk_TypeSafeClient_Retry"></a>

### Retry


```csharp
public RetryPolicy Retry { get; }
```


Retry settings with constructor overrides applied.

**Value:** [RetryPolicy](/api/TypeSafe.AI.Sdk/RetryPolicy/)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/TypeSafeClient.cs#L45)

<a id="TypeSafe_AI_Sdk_TypeSafeClient_TimeoutMs_"></a>
<a id="TypeSafe_AI_Sdk_TypeSafeClient_TimeoutMs"></a>

### TimeoutMs


```csharp
public int TimeoutMs { get; }
```


Timeout per attempt in milliseconds.

**Value:** [int](https://learn.microsoft.com/dotnet/api/system.int32)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/TypeSafeClient.cs#L48)

## Methods

<a id="TypeSafe_AI_Sdk_TypeSafeClient_Dispose_"></a>
<a id="TypeSafe_AI_Sdk_TypeSafeClient_Dispose"></a>

### Dispose()


```csharp
public void Dispose()
```


Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/TypeSafeClient.cs#L511)

<a id="TypeSafe_AI_Sdk_TypeSafeClient_SystemOneAsync_"></a>
<a id="TypeSafe_AI_Sdk_TypeSafeClient_SystemOneAsync_System_Object_System_Collections_Generic_IDictionary_System_String_TypeSafe_AI_Sdk_Question__System_String_TypeSafe_AI_Sdk_RequestOptions_System_Threading_CancellationToken_"></a>

### `SystemOneAsync(object?, IDictionary<string, Question>, string?, RequestOptions?, CancellationToken)`


```csharp
public Task<SystemOneResult> SystemOneAsync(object? state, IDictionary<string, Question> questions, string? model = null, RequestOptions? options = null, CancellationToken cancellationToken = default)
```


Answer named questions about text or structured state.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `state` | [object](https://learn.microsoft.com/dotnet/api/system.object) |  |
| `questions` | [IDictionary&lt;string, Question&gt;](https://learn.microsoft.com/dotnet/api/system.collections.generic.idictionary-2) |  |
| `model` | [string](https://learn.microsoft.com/dotnet/api/system.string) |  |
| `options` | [RequestOptions](/api/TypeSafe.AI.Sdk/RequestOptions/) |  |
| `cancellationToken` | [CancellationToken](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) |  |

**Returns:** [Task&lt;SystemOneResult&gt;](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/TypeSafeClient.cs#L166)

<a id="TypeSafe_AI_Sdk_TypeSafeClient_SystemOneAsync_System_Object_System_ValueTuple_System_String_TypeSafe_AI_Sdk_Question____"></a>

### SystemOneAsync(object?, params (string Name, Question Question)[])


```csharp
public Task<SystemOneResult> SystemOneAsync(object? state, params (string Name, Question Question)[] questions)
```


Answer named questions about text or structured state.
Duplicate names throw. Use the dictionary overload for `model`, options, or a cancellation token.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `state` | [object](https://learn.microsoft.com/dotnet/api/system.object) |  |
| `questions` | [(string Name, Question Question)\[\]](https://learn.microsoft.com/dotnet/api/system.string) |  |

**Returns:** [Task&lt;SystemOneResult&gt;](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/TypeSafeClient.cs#L175)

<a id="TypeSafe_AI_Sdk_TypeSafeClient_SystemOneAsync_TypeSafe_AI_Sdk_SystemOneRequest_TypeSafe_AI_Sdk_RequestOptions_System_Threading_CancellationToken_"></a>

### SystemOneAsync(SystemOneRequest, RequestOptions?, CancellationToken)


```csharp
public Task<SystemOneResult> SystemOneAsync(SystemOneRequest request, RequestOptions? options = null, CancellationToken cancellationToken = default)
```


Answer named questions about text or structured state.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `request` | [SystemOneRequest](/api/TypeSafe.AI.Sdk/SystemOneRequest/) |  |
| `options` | [RequestOptions](/api/TypeSafe.AI.Sdk/RequestOptions/) |  |
| `cancellationToken` | [CancellationToken](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) |  |

**Returns:** [Task&lt;SystemOneResult&gt;](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/TypeSafeClient.cs#L123)

<a id="TypeSafe_AI_Sdk_TypeSafeClient_SystemOneWithResponseAsync_"></a>
<a id="TypeSafe_AI_Sdk_TypeSafeClient_SystemOneWithResponseAsync_System_Object_System_Collections_Generic_IDictionary_System_String_TypeSafe_AI_Sdk_Question__System_String_TypeSafe_AI_Sdk_RequestOptions_System_Threading_CancellationToken_"></a>

### `SystemOneWithResponseAsync(object?, IDictionary<string, Question>, string?, RequestOptions?, CancellationToken)`


```csharp
public Task<TypeSafeResponse<SystemOneResult>> SystemOneWithResponseAsync(object? state, IDictionary<string, Question> questions, string? model = null, RequestOptions? options = null, CancellationToken cancellationToken = default)
```


Answer named questions and return the HTTP response metadata.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `state` | [object](https://learn.microsoft.com/dotnet/api/system.object) |  |
| `questions` | [IDictionary&lt;string, Question&gt;](https://learn.microsoft.com/dotnet/api/system.collections.generic.idictionary-2) |  |
| `model` | [string](https://learn.microsoft.com/dotnet/api/system.string) |  |
| `options` | [RequestOptions](/api/TypeSafe.AI.Sdk/RequestOptions/) |  |
| `cancellationToken` | [CancellationToken](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) |  |

**Returns:** [Task&lt;TypeSafeResponse&lt;SystemOneResult&gt;&gt;](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/TypeSafeClient.cs#L157)

<a id="TypeSafe_AI_Sdk_TypeSafeClient_SystemOneWithResponseAsync_System_Object_System_ValueTuple_System_String_TypeSafe_AI_Sdk_Question____"></a>

### SystemOneWithResponseAsync(object?, params (string Name, Question Question)[])


```csharp
public Task<TypeSafeResponse<SystemOneResult>> SystemOneWithResponseAsync(object? state, params (string Name, Question Question)[] questions)
```


Answer named questions and return the HTTP response metadata.
Duplicate names throw. Use the dictionary overload for `model`, options, or a cancellation token.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `state` | [object](https://learn.microsoft.com/dotnet/api/system.object) |  |
| `questions` | [(string Name, Question Question)\[\]](https://learn.microsoft.com/dotnet/api/system.string) |  |

**Returns:** [Task&lt;TypeSafeResponse&lt;SystemOneResult&gt;&gt;](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/TypeSafeClient.cs#L181)

<a id="TypeSafe_AI_Sdk_TypeSafeClient_SystemOneWithResponseAsync_TypeSafe_AI_Sdk_SystemOneRequest_TypeSafe_AI_Sdk_RequestOptions_System_Threading_CancellationToken_"></a>

### SystemOneWithResponseAsync(SystemOneRequest, RequestOptions?, CancellationToken)


```csharp
public Task<TypeSafeResponse<SystemOneResult>> SystemOneWithResponseAsync(SystemOneRequest request, RequestOptions? options = null, CancellationToken cancellationToken = default)
```


Answer named questions and return the HTTP response metadata.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `request` | [SystemOneRequest](/api/TypeSafe.AI.Sdk/SystemOneRequest/) |  |
| `options` | [RequestOptions](/api/TypeSafe.AI.Sdk/RequestOptions/) |  |
| `cancellationToken` | [CancellationToken](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) |  |

**Returns:** [Task&lt;TypeSafeResponse&lt;SystemOneResult&gt;&gt;](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/TypeSafeClient.cs#L140)
