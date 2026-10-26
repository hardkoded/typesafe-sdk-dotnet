# Images

Image support is experimental. Opt in by sending the `TypeSafe-Danger-Zone-Images: true` header:

```csharp
using var client = new TypeSafeClient(new TypeSafeClientOptions
{
    DefaultHeaders = new Dictionary<string, string> { ["TypeSafe-Danger-Zone-Images"] = "true" },
});
```

To send it with one request instead, pass it in `RequestOptions.Headers`.

Create an [Image](/api/TypeSafe.AI.Sdk/Image/) from file bytes, `Image.FromFile`, or `Image.FromBase64`. Images must be PNG or JPEG. The API detects the format from the file bytes and rejects other formats, such as WebP and GIF, with a [BadRequestException](/api/TypeSafe.AI.Sdk/BadRequestException/). There are three ways to use an image in a request, and you can combine them. A request with images is sent as `multipart/form-data`, with each image's file bytes in its own part. Each image is sent once, however many times you use it.

## Place the image in the content

Put the image anywhere in `State`, question instructions, or criteria:

```csharp
var photo = Image.FromFile("returns/RMA-1042.jpg");

var response = await client.SystemOneAsync(
    new { claim = "The mug arrived with a chipped rim.", photo },
    ("damaged", Question.Noul("Is the product in the photo damaged?")),
    ("severity", Question.Score("How severe is the damage?", "none", "cosmetic", "unusable")));
```

## Write the image into text

Use `TextWithAssets.From` with an interpolated string to reference images inside text:

```csharp
var upload = Image.FromFile("upload.png");
var catalog = Image.FromFile("catalog.jpg");

var response = await client.SystemOneAsync(
    TextWithAssets.From($"A seller uploaded {upload} for a listing whose catalog photo is {catalog}."),
    ("sameProduct", Question.Noul(TextWithAssets.From($"Does {upload} show the same product as {catalog}?"))));
```

An image in a plain interpolated string, such as `$"Compare {upload}"`, makes the call throw a [TypeSafeException](/api/TypeSafe.AI.Sdk/TypeSafeException/) before the request is sent. To write a list of images, use `TextWithAssets.Join(images, ", ")`.

## Choose your own references

Pass images in `SystemOneRequest.Assets`, keyed by a reference of your choice, to reference them from plain strings such as prompts stored in configuration:

```csharp
var response = await client.SystemOneAsync(new SystemOneRequest
{
    State = "A customer returned <delivered>. The catalog photo is <catalog>.",
    Questions = Question.Map(("matches", Question.Noul("Is <delivered> the product shown in <catalog>?"))),
    Assets = new Dictionary<string, Image>
    {
        ["<delivered>"] = Image.FromFile("returns/RMA-1042.jpg"),
        ["<catalog>"] = Image.FromFile("catalog/mug.png"),
    },
});
```

Use distinct delimiters, such as `<delivered>`, so references don't match ordinary text. An image in `Assets` that is also placed in content or in `TextWithAssets` is written there as its name. Every name in `Assets` must appear in the request; otherwise the call throws a [TypeSafeException](/api/TypeSafe.AI.Sdk/TypeSafeException/) before sending.

<a id="locate-something-with-box"></a>

## Locate something with `Box`

Ask a `Question.Box` question to locate what its instructions describe. The answer's `Box` holds the edges as fractions of the image's height (`Top`, `Bottom`) and width (`Left`, `Right`), and its `Confidence` is `null` when the API doesn't report one. Use the `jev-preview` model for box questions:

```csharp
var response = await client.SystemOneAsync(new SystemOneRequest
{
    State = Image.FromFile("returns/RMA-1042.jpg"),
    Questions = Question.Map(("damage", Question.Box("The damaged area of the product"))),
    Model = "jev-preview",
});
var box = response.GetBox("damage").Box;
Console.WriteLine($"{box.Left},{box.Top} to {box.Right},{box.Bottom}");
```
