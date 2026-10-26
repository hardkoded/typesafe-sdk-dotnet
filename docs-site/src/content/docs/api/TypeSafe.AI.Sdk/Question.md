---
title: "Question"
description: "A TypeSafe question identified by its type field."
editUrl: false
prev: false
next: false
sidebar:
  label: "Question"
  hidden: true
---

<a id="TypeSafe_AI_Sdk_Question"></a>

Namespace: [TypeSafe.AI.Sdk](/api/TypeSafe.AI.Sdk/)


```csharp
public abstract class Question
```


A TypeSafe question identified by its `type` field.

**Inheritance:** [object](https://learn.microsoft.com/dotnet/api/system.object)

**Derived:** [BoxQuestion](/api/TypeSafe.AI.Sdk/BoxQuestion/), [ChoiceQuestion](/api/TypeSafe.AI.Sdk/ChoiceQuestion/), [NoulQuestion](/api/TypeSafe.AI.Sdk/NoulQuestion/), [ScoreQuestion](/api/TypeSafe.AI.Sdk/ScoreQuestion/)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Questions/Questions.cs#L10)

## Properties

<a id="TypeSafe_AI_Sdk_Question_Instructions_"></a>
<a id="TypeSafe_AI_Sdk_Question_Instructions"></a>

### Instructions


```csharp
public object? Instructions { get; }
```


The question as text, a JSON object, or an array. Omitted when the builder was not given
instructions (except `NoulCriteria)`, which defaults to `null`).

**Value:** [object](https://learn.microsoft.com/dotnet/api/system.object)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Questions/Questions.cs#L19)

<a id="TypeSafe_AI_Sdk_Question_Type_"></a>
<a id="TypeSafe_AI_Sdk_Question_Type"></a>

### Type


```csharp
public abstract string Type { get; }
```


Question type: `noul`, `choice`, `score`, or `box`.

**Value:** [string](https://learn.microsoft.com/dotnet/api/system.string)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Questions/Questions.cs#L13)

## Methods

<a id="TypeSafe_AI_Sdk_Question_Box_"></a>
<a id="TypeSafe_AI_Sdk_Question_Box_System_Object_"></a>

### Box(object?)


```csharp
public static BoxQuestion Box(object? instructions = null)
```


Create a question that locates what its instructions describe in the request's image.
Mirrors JS `box()`. Use the `jev-preview` model for box questions.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `instructions` | [object](https://learn.microsoft.com/dotnet/api/system.object) | What to locate, as text, an asset, a JSON object or array; defaults to `null`. |

**Returns:** [BoxQuestion](/api/TypeSafe.AI.Sdk/BoxQuestion/)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Questions/Questions.cs#L107)

<a id="TypeSafe_AI_Sdk_Question_Choice_"></a>
<a id="TypeSafe_AI_Sdk_Question_Choice_System_Object_System_Collections_Generic_Dictionary_System_String_System_Object__"></a>

### `Choice(object?, Dictionary<string, object?>)`


```csharp
public static ChoiceQuestion Choice(object? instructions, Dictionary<string, object?> criteria)
```


Create a question that selects between named alternatives.
Mirrors JS `choice()`.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `instructions` | [object](https://learn.microsoft.com/dotnet/api/system.object) | The question as text, a JSON object or array, or `null`. |
| `criteria` | [Dictionary&lt;string, object&gt;](https://learn.microsoft.com/dotnet/api/system.collections.generic.dictionary-2) | Labels mapped to descriptions, or `null` for undescribed labels. |

**Returns:** [ChoiceQuestion](/api/TypeSafe.AI.Sdk/ChoiceQuestion/)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Questions/Questions.cs#L61)

<a id="TypeSafe_AI_Sdk_Question_Choice_System_Object_System_Collections_Generic_IReadOnlyDictionary_System_String_System_Object__"></a>

### `Choice(object?, IReadOnlyDictionary<string, object?>)`


```csharp
public static ChoiceQuestion Choice(object? instructions, IReadOnlyDictionary<string, object?> criteria)
```


Create a question that selects between named alternatives.
Mirrors JS `choice()`.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `instructions` | [object](https://learn.microsoft.com/dotnet/api/system.object) | The question as text, a JSON object or array, or `null`. |
| `criteria` | [IReadOnlyDictionary&lt;string, object&gt;](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlydictionary-2) | Labels mapped to descriptions, or `null` for undescribed labels. |

**Returns:** [ChoiceQuestion](/api/TypeSafe.AI.Sdk/ChoiceQuestion/)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Questions/Questions.cs#L50)

<a id="TypeSafe_AI_Sdk_Question_Choice_System_Object_System_ValueTuple_System_String_System_Object____"></a>

### Choice(object?, params (string Label, object? Description)[])


```csharp
public static ChoiceQuestion Choice(object? instructions, params (string Label, object? Description)[] criteria)
```


Create a question that selects between named alternatives, from label/description pairs.
Duplicate labels throw. Pass a dictionary when you already have a map.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `instructions` | [object](https://learn.microsoft.com/dotnet/api/system.object) |  |
| `criteria` | [(string Label, object Description)\[\]](https://learn.microsoft.com/dotnet/api/system.string) |  |

**Returns:** [ChoiceQuestion](/api/TypeSafe.AI.Sdk/ChoiceQuestion/)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Questions/Questions.cs#L68)

<a id="TypeSafe_AI_Sdk_Question_Map_"></a>
<a id="TypeSafe_AI_Sdk_Question_Map_System_ValueTuple_System_String_TypeSafe_AI_Sdk_Question____"></a>

### Map(params (string Name, Question Question)[])


```csharp
public static Dictionary<string, Question> Map(params (string Name, Question Question)[] questions)
```


Build a name → question map from labeled pairs. Duplicate names throw.
Use this when you need a dictionary (for example [Questions](/api/TypeSafe.AI.Sdk/SystemOneRequest/#TypeSafe_AI_Sdk_SystemOneRequest_Questions),
or a `model` / cancellation token on the dictionary `SystemOneAsync` overload).

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `questions` | [(string Name, Question Question)\[\]](https://learn.microsoft.com/dotnet/api/system.string) |  |

**Returns:** [Dictionary&lt;string, Question&gt;](https://learn.microsoft.com/dotnet/api/system.collections.generic.dictionary-2)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Questions/Questions.cs#L78)

<a id="TypeSafe_AI_Sdk_Question_Noul_"></a>
<a id="TypeSafe_AI_Sdk_Question_Noul_System_Object_TypeSafe_AI_Sdk_NoulCriteria_"></a>

### Noul(object?, NoulCriteria?)


```csharp
public static NoulQuestion Noul(object? instructions = null, NoulCriteria? criteria = null)
```


Create a yes/no question with optional descriptions for either outcome.
Mirrors JS `noul()`.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `instructions` | [object](https://learn.microsoft.com/dotnet/api/system.object) | The question as text, a JSON object or array; defaults to `null`. |
| `criteria` | [NoulCriteria](/api/TypeSafe.AI.Sdk/NoulCriteria/) | Optional descriptions of the yes and no outcomes. |

**Returns:** [NoulQuestion](/api/TypeSafe.AI.Sdk/NoulQuestion/)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Questions/Questions.cs#L35)

<a id="TypeSafe_AI_Sdk_Question_Noul_System_Object_TypeSafe_AI_Sdk_NoulCriteria_System_Boolean_"></a>

### Noul(object?, NoulCriteria?, bool)


```csharp
public static NoulQuestion Noul(object? instructions, NoulCriteria? criteria, bool includeCriteria)
```


Create a yes/no question, including an explicit `null` criteria value on the wire.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `instructions` | [object](https://learn.microsoft.com/dotnet/api/system.object) |  |
| `criteria` | [NoulCriteria](/api/TypeSafe.AI.Sdk/NoulCriteria/) |  |
| `includeCriteria` | [bool](https://learn.microsoft.com/dotnet/api/system.boolean) |  |

**Returns:** [NoulQuestion](/api/TypeSafe.AI.Sdk/NoulQuestion/)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Questions/Questions.cs#L41)

<a id="TypeSafe_AI_Sdk_Question_Score_"></a>
<a id="TypeSafe_AI_Sdk_Question_Score_System_Object_System_Collections_Generic_IReadOnlyList_System_Object__"></a>

### `Score(object?, IReadOnlyList<object?>)`


```csharp
public static ScoreQuestion Score(object? instructions, IReadOnlyList<object?> criteria)
```


Create a score question using an ordered rubric.
Mirrors JS `score()`.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `instructions` | [object](https://learn.microsoft.com/dotnet/api/system.object) | The question as text, a JSON object or array, or `null`. |
| `criteria` | [IReadOnlyList&lt;object&gt;](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlylist-1) | At least two descriptions indexed by score from zero; entries may be `null`. |

**Returns:** [ScoreQuestion](/api/TypeSafe.AI.Sdk/ScoreQuestion/)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Questions/Questions.cs#L87)

<a id="TypeSafe_AI_Sdk_Question_Score_System_Object_System_Object___"></a>

### Score(object?, params object?[])


```csharp
public static ScoreQuestion Score(object? instructions, params object?[] criteria)
```


Create a score question using an ordered rubric.
Mirrors JS `score()`.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `instructions` | [object](https://learn.microsoft.com/dotnet/api/system.object) | The question as text, a JSON object or array, or `null`. |
| `criteria` | [object\[\]](https://learn.microsoft.com/dotnet/api/system.object) | At least two descriptions indexed by score from zero; entries may be `null`. |

**Returns:** [ScoreQuestion](/api/TypeSafe.AI.Sdk/ScoreQuestion/)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Questions/Questions.cs#L99)

<a id="TypeSafe_AI_Sdk_Question_Validate_"></a>
<a id="TypeSafe_AI_Sdk_Question_Validate_System_Collections_Generic_IReadOnlyDictionary_System_String_TypeSafe_AI_Sdk_Question__"></a>

### `Validate(IReadOnlyDictionary<string, Question>)`


```csharp
public static void Validate(IReadOnlyDictionary<string, Question> questions)
```


Reject empty question sets and score questions without a list of at least two criteria.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `questions` | [IReadOnlyDictionary&lt;string, Question&gt;](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlydictionary-2) |  |

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Questions/Questions.cs#L110)
