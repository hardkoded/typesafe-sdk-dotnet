---
title: "Usage"
description: "Token usage for a request."
editUrl: false
prev: false
next: false
sidebar:
  label: "Usage"
  hidden: true
---

<a id="TypeSafe_AI_Sdk_Usage"></a>

Namespace: [TypeSafe.AI.Sdk](/api/TypeSafe.AI.Sdk/)


```csharp
public sealed class Usage
```


Token usage for a request.

**Inheritance:** [object](https://learn.microsoft.com/dotnet/api/system.object)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Responses/Responses.cs#L10)

## Properties

<a id="TypeSafe_AI_Sdk_Usage_InputTokens_"></a>
<a id="TypeSafe_AI_Sdk_Usage_InputTokens"></a>

### InputTokens


```csharp
[JsonPropertyName("input_tokens")]
public int? InputTokens { get; init; }
```


Number of input tokens used, or `null` when the API does not report it.

**Value:** [int?](https://learn.microsoft.com/dotnet/api/system.int32)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Responses/Responses.cs#L13)

<a id="TypeSafe_AI_Sdk_Usage_OutputTokens_"></a>
<a id="TypeSafe_AI_Sdk_Usage_OutputTokens"></a>

### OutputTokens


```csharp
[JsonPropertyName("output_tokens")]
public int? OutputTokens { get; init; }
```


Number of output tokens used, or `null` when the API does not report it.

**Value:** [int?](https://learn.microsoft.com/dotnet/api/system.int32)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Responses/Responses.cs#L17)
