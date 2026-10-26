---
title: "ModelsResource"
description: "Access to the Models API resource."
editUrl: false
prev: false
next: false
sidebar:
  label: "ModelsResource"
  hidden: true
---

<a id="TypeSafe_AI_Sdk_ModelsResource"></a>

Namespace: [TypeSafe.AI.Sdk](/api/TypeSafe.AI.Sdk/)


```csharp
public sealed class ModelsResource
```


Access to the Models API resource.

**Inheritance:** [object](https://learn.microsoft.com/dotnet/api/system.object)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Models/ModelsResource.cs#L7)

## Methods

<a id="TypeSafe_AI_Sdk_ModelsResource_ListAsync_"></a>
<a id="TypeSafe_AI_Sdk_ModelsResource_ListAsync_TypeSafe_AI_Sdk_RequestOptions_System_Threading_CancellationToken_"></a>

### ListAsync(RequestOptions?, CancellationToken)


```csharp
public Task<IReadOnlyList<ModelCard>> ListAsync(RequestOptions? options = null, CancellationToken cancellationToken = default)
```


List the models available to the account.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `options` | [RequestOptions](/api/TypeSafe.AI.Sdk/RequestOptions/) |  |
| `cancellationToken` | [CancellationToken](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) |  |

**Returns:** [Task&lt;IReadOnlyList&lt;ModelCard&gt;&gt;](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Models/ModelsResource.cs#L17)

<a id="TypeSafe_AI_Sdk_ModelsResource_ListWithResponseAsync_"></a>
<a id="TypeSafe_AI_Sdk_ModelsResource_ListWithResponseAsync_TypeSafe_AI_Sdk_RequestOptions_System_Threading_CancellationToken_"></a>

### ListWithResponseAsync(RequestOptions?, CancellationToken)


```csharp
public Task<TypeSafeResponse<IReadOnlyList<ModelCard>>> ListWithResponseAsync(RequestOptions? options = null, CancellationToken cancellationToken = default)
```


List models and return the HTTP response metadata.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `options` | [RequestOptions](/api/TypeSafe.AI.Sdk/RequestOptions/) |  |
| `cancellationToken` | [CancellationToken](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken) |  |

**Returns:** [Task&lt;TypeSafeResponse&lt;IReadOnlyList&lt;ModelCard&gt;&gt;&gt;](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Models/ModelsResource.cs#L29)
