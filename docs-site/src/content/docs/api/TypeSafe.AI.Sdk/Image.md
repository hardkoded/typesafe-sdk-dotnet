---
title: "Image"
description: "A PNG or JPEG image for CancellationToken). Image support is experimental: send the TypeSafe-Danger-Zone-Images: true header to opt in."
editUrl: false
prev: false
next: false
sidebar:
  label: "Image"
  hidden: true
---

<a id="TypeSafe_AI_Sdk_Image"></a>

Namespace: [TypeSafe.AI.Sdk](/api/TypeSafe.AI.Sdk/)


```csharp
public sealed class Image
```


A PNG or JPEG image for `CancellationToken)`.
Image support is experimental: send the `TypeSafe-Danger-Zone-Images: true` header to opt in.

**Inheritance:** [object](https://learn.microsoft.com/dotnet/api/system.object)

Place the image in [State](/api/TypeSafe.AI.Sdk/SystemOneRequest/#TypeSafe_AI_Sdk_SystemOneRequest_State) or in question instructions or criteria, or interpolate it
into [From(FormattableString)](/api/TypeSafe.AI.Sdk/TextWithAssets/#TypeSafe_AI_Sdk_TextWithAssets_From_System_FormattableString_); either way it is sent under its own reference,
[ToString()](#TypeSafe_AI_Sdk_Image_ToString). To reference it by a name of your choice, add it to [Assets](/api/TypeSafe.AI.Sdk/SystemOneRequest/#TypeSafe_AI_Sdk_SystemOneRequest_Assets).
The API detects the format from the file bytes and rejects formats other than PNG and JPEG with a
[BadRequestException](/api/TypeSafe.AI.Sdk/BadRequestException/).

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Assets/Image.cs#L19)

## Constructors

<a id="TypeSafe_AI_Sdk_Image__ctor_"></a>
<a id="TypeSafe_AI_Sdk_Image__ctor_System_ReadOnlyMemory_System_Byte__"></a>

### `Image(ReadOnlyMemory<byte>)`


```csharp
public Image(ReadOnlyMemory<byte> data)
```


Create an image from file bytes. The bytes are copied.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `data` | [ReadOnlyMemory&lt;byte&gt;](https://learn.microsoft.com/dotnet/api/system.readonlymemory-1) |  |

**Exceptions**

| Type | Condition |
| --- | --- |
| [TypeSafeException](/api/TypeSafe.AI.Sdk/TypeSafeException/) | &lt;code class="paramref"&gt;data&lt;/code&gt; is empty. |

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Assets/Image.cs#L26)

## Properties

<a id="TypeSafe_AI_Sdk_Image_Data_"></a>
<a id="TypeSafe_AI_Sdk_Image_Data"></a>

### Data


```csharp
public ReadOnlyMemory<byte> Data { get; }
```


The image file bytes.

**Value:** [ReadOnlyMemory&lt;byte&gt;](https://learn.microsoft.com/dotnet/api/system.readonlymemory-1)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Assets/Image.cs#L37)

## Methods

<a id="TypeSafe_AI_Sdk_Image_FromBase64_"></a>
<a id="TypeSafe_AI_Sdk_Image_FromBase64_System_String_"></a>

### FromBase64(string)


```csharp
public static Image FromBase64(string text)
```


Create an image from base64-encoded file bytes, without a `data:` URL prefix.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `text` | [string](https://learn.microsoft.com/dotnet/api/system.string) |  |

**Returns:** [Image](./)

**Exceptions**

| Type | Condition |
| --- | --- |
| [TypeSafeException](/api/TypeSafe.AI.Sdk/TypeSafeException/) | &lt;code class="paramref"&gt;text&lt;/code&gt; isn't valid base64 or decodes to no bytes. |

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Assets/Image.cs#L43)

<a id="TypeSafe_AI_Sdk_Image_FromFile_"></a>
<a id="TypeSafe_AI_Sdk_Image_FromFile_System_String_"></a>

### FromFile(string)


```csharp
public static Image FromFile(string path)
```


Create an image from the bytes of a file, read now.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `path` | [string](https://learn.microsoft.com/dotnet/api/system.string) |  |

**Returns:** [Image](./)

**Exceptions**

| Type | Condition |
| --- | --- |
| [TypeSafeException](/api/TypeSafe.AI.Sdk/TypeSafeException/) | The file is empty or can't be read. |
| [ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception) | &lt;code class="paramref"&gt;path&lt;/code&gt; is null or empty. |

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Assets/Image.cs#L58)

<a id="TypeSafe_AI_Sdk_Image_ToString_"></a>
<a id="TypeSafe_AI_Sdk_Image_ToString"></a>

### ToString()


```csharp
public override string ToString()
```


The unique reference this image is sent under, for referencing it inside text.

**Returns:** [string](https://learn.microsoft.com/dotnet/api/system.string)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Assets/Image.cs#L74)
