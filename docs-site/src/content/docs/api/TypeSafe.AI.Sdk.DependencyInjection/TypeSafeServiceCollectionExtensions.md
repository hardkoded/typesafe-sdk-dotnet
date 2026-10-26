---
title: "TypeSafeServiceCollectionExtensions"
description: "Dependency-injection helpers for TypeSafeClient."
editUrl: false
prev: false
next: false
sidebar:
  label: "TypeSafeServiceCollectionExtensions"
  hidden: true
---

<a id="TypeSafe_AI_Sdk_DependencyInjection_TypeSafeServiceCollectionExtensions"></a>

Namespace: [TypeSafe.AI.Sdk.DependencyInjection](/api/TypeSafe.AI.Sdk.DependencyInjection/)


```csharp
public static class TypeSafeServiceCollectionExtensions
```


Dependency-injection helpers for [TypeSafeClient](/api/TypeSafe.AI.Sdk/TypeSafeClient/).

**Inheritance:** [object](https://learn.microsoft.com/dotnet/api/system.object)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/DependencyInjection/TypeSafeServiceCollectionExtensions.cs#L12)

## Methods

<a id="TypeSafe_AI_Sdk_DependencyInjection_TypeSafeServiceCollectionExtensions_AddTypeSafeClient_"></a>
<a id="TypeSafe_AI_Sdk_DependencyInjection_TypeSafeServiceCollectionExtensions_AddTypeSafeClient_Microsoft_Extensions_DependencyInjection_IServiceCollection_System_Action_TypeSafe_AI_Sdk_TypeSafeClientOptions__"></a>

### `AddTypeSafeClient(IServiceCollection, Action<TypeSafeClientOptions>?)`


```csharp
public static IHttpClientBuilder AddTypeSafeClient(this IServiceCollection services, Action<TypeSafeClientOptions>? configure = null)
```


Register [ITypeSafeClient](/api/TypeSafe.AI.Sdk/ITypeSafeClient/) / [TypeSafeClient](/api/TypeSafe.AI.Sdk/TypeSafeClient/) using
[IHttpClientFactory](https://learn.microsoft.com/dotnet/api/system.net.http.ihttpclientfactory). Configuration still reads `TYPESAFE_API_KEY`
and the other [Env](/api/TypeSafe.AI.Sdk/Env/) variables when options omit them.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `services` | [IServiceCollection](https://learn.microsoft.com/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection) |  |
| `configure` | [Action&lt;TypeSafeClientOptions&gt;](https://learn.microsoft.com/dotnet/api/system.action-1) |  |

**Returns:** [IHttpClientBuilder](https://learn.microsoft.com/dotnet/api/microsoft.extensions.dependencyinjection.ihttpclientbuilder)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/DependencyInjection/TypeSafeServiceCollectionExtensions.cs#L19)
