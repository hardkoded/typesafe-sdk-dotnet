// Copyright (c) Dario Kondratiuk.
// Licensed under the MIT License.

using System.Text;
using System.Text.Json;
using Xunit;

namespace TypeSafe.AI.Sdk.Tests;

public sealed class ImageAssetTests
{
    private const string PngBase64 =
        "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mNkYAAAAAYAAjCB0C8AAAAASUVORK5CYII=";

    private static byte[] PngBytes() => Convert.FromBase64String(PngBase64);

    private static Image Png() => new(PngBytes());

    [Fact]
    public void Reference_is_unique_lowercase_hex()
    {
        var photo = Png();
        var listing = Png();
        Assert.Matches(@"^<image:[0-9a-f]{32}>$", photo.ToString());
        Assert.NotEqual(photo.ToString(), listing.ToString());
        Assert.Equal("image", photo.Type);
        Assert.Equal(photo.ToString(), JsonSerializer.Serialize(photo).Trim('"'));
    }

    [Fact]
    public void FromBase64_decodes_png_bytes()
    {
        var image = Image.FromBase64(PngBase64);
        Assert.Equal(PngBytes(), image.Data.ToArray());
    }

    [Fact]
    public void FromBase64_rejects_invalid_or_empty_text()
    {
        var invalid = Assert.Throws<TypeSafeException>(() => Image.FromBase64("not base64!"));
        Assert.Contains("Image data must contain valid base64 text.", invalid.Message);

        var dataUrl = Assert.Throws<TypeSafeException>(() => Image.FromBase64($"data:image/png;base64,{PngBase64}"));
        Assert.Contains("Image data must contain valid base64 text.", dataUrl.Message);

        var empty = Assert.Throws<TypeSafeException>(() => Image.FromBase64(""));
        Assert.Contains("Image data is empty.", empty.Message);
    }

    [Fact]
    public void Rejects_empty_data_from_bytes_memory_and_stream()
    {
        Assert.Contains("Image data is empty.", Assert.Throws<TypeSafeException>(() => new Image(Array.Empty<byte>())).Message);
        Assert.Contains("Image data is empty.", Assert.Throws<TypeSafeException>(() => new Image(ReadOnlyMemory<byte>.Empty)).Message);
        Assert.Contains("Image data is empty.", Assert.Throws<TypeSafeException>(() => new Image(Stream.Null)).Message);
        Assert.Contains("Image data must be file bytes.", Assert.Throws<TypeSafeException>(() => new Image((byte[])null!)).Message);
    }

    [Fact]
    public void ReadOnlyMemory_uses_only_the_viewed_bytes()
    {
        var png = PngBytes();
        var padded = new byte[png.Length + 2];
        padded[0] = 0;
        png.CopyTo(padded, 1);
        padded[^1] = 0;
        var image = new Image(padded.AsMemory(1, png.Length));
        Assert.Equal(png, image.Data.ToArray());
    }

    [Fact]
    public void Stream_is_read_at_construction()
    {
        using var stream = new MemoryStream(PngBytes());
        var image = new Image(stream);
        Assert.Equal(PngBytes(), image.Data.ToArray());
    }

    [Fact]
    public void WithAssets_attaches_only_images_whose_references_appear_in_the_text()
    {
        var photo = Png();
        var listing = Png();
        var cycle = new Dictionary<string, object?> { ["photo"] = photo };
        cycle["self"] = cycle;

        Assert.Empty(WithAssets.Format("{0} {1}", new Dictionary<string, object?> { ["photo"] = photo }, cycle).Assets);
        Assert.Equal(new[] { photo, listing }, WithAssets.Join(new object[] { photo, listing }).Assets);
        Assert.Equal(new[] { photo }, WithAssets.Format("{0} {1}", WithAssets.Format($"{photo}"), new Dictionary<string, object?> { ["listing"] = listing }).Assets);
    }

    [Fact]
    public void Join_writes_values_with_a_separator_and_nested_text()
    {
        var photo = Png();
        var listing = Png();
        var joined = WithAssets.Join(new object?[] { photo, "and", WithAssets.Format($"{listing}"), null, 1 }, " ");
        Assert.Equal($"{photo} and {listing}  1", joined.Text);
        Assert.Equal(new[] { photo, listing }, joined.Assets);
        Assert.Equal($"{photo},{listing}", WithAssets.Join(new object[] { photo, listing }).Text);
        Assert.Equal("", WithAssets.Join(Array.Empty<object>()).Text);
        Assert.Equal(
            new[] { photo, listing },
            WithAssets.Format($"Compare {WithAssets.Join(new object[] { photo, listing }, ", ")}").Assets);
    }

    [Fact]
    public async Task Sends_assets_keyed_by_named_reference()
    {
        var handler = ScriptedHandler.Json(Fixtures.SystemOne);
        using var client = TestClient.Create(handler);
        await client.SystemOneAsync(new SystemOneRequest
        {
            State = "Inspect this returned product: <product>",
            Questions = new Dictionary<string, Question> { ["q1"] = Question.Noul("Is the product damaged?") },
            Assets = new Dictionary<string, Image> { ["<product>"] = Png() },
        });

        var body = handler.Requests[0].Body!.Value;
        Assert.Equal("Inspect this returned product: <product>", body.GetProperty("state").GetString());
        Assert.Equal("asset-0", body.GetProperty("assets").GetProperty("<product>").GetProperty("part").GetString());
        Assert.Equal("image", body.GetProperty("assets").GetProperty("<product>").GetProperty("type").GetString());
        Assert.Equal(PngBytes(), Part(handler.Requests[0], "asset-0").Bytes);
        Assert.Equal("image/png", Part(handler.Requests[0], "asset-0").ContentType);
        Assert.False(handler.Requests[0].Headers.ContainsKey("TypeSafe-Danger-Zone-Images"));
    }

    [Fact]
    public async Task Sends_images_in_state_and_question_content_under_their_references()
    {
        var photo = Png();
        var listing = Png();
        var handler = ScriptedHandler.Json(Fixtures.SystemOne);
        using var client = TestClient.Create(handler);
        await client.SystemOneAsync(new { photo }, new Dictionary<string, Question>
        {
            ["q1"] = Question.Noul(new object[] { "Does it match the listing?", listing }),
        });

        var body = handler.Requests[0].Body!.Value;
        Assert.Equal(photo.ToString(), body.GetProperty("state").GetProperty("photo").GetString());
        Assert.Equal(listing.ToString(), body.GetProperty("questions").GetProperty("q1").GetProperty("instructions")[1].GetString());
        Assert.Equal("asset-0", body.GetProperty("assets").GetProperty(photo.ToString()).GetProperty("part").GetString());
        Assert.Equal("asset-1", body.GetProperty("assets").GetProperty(listing.ToString()).GetProperty("part").GetString());
        Assert.Equal(new[] { "request", "asset-0", "asset-1" }, handler.Requests[0].Parts.Select(p => p.Name).ToArray());
    }

    [Fact]
    public async Task Sends_images_in_WithAssets_text_including_nested_text()
    {
        var photo = Png();
        var listing = Png();
        var handler = ScriptedHandler.Json(Fixtures.SystemOne);
        using var client = TestClient.Create(handler);
        await client.SystemOneAsync(new SystemOneRequest
        {
            State = WithAssets.Format($"Compare {photo} with {WithAssets.Format($"listing {listing}")} and <logo> ({1})"),
            Questions = new Dictionary<string, Question>
            {
                ["q1"] = Question.Score(WithAssets.Format($"Rate {photo}"), "bad", "good"),
            },
            Assets = new Dictionary<string, Image> { ["<logo>"] = Png() },
        });

        var body = handler.Requests[0].Body!.Value;
        Assert.Equal($"Compare {photo} with listing {listing} and <logo> (1)", body.GetProperty("state").GetString());
        Assert.Equal($"Rate {photo}", body.GetProperty("questions").GetProperty("q1").GetProperty("instructions").GetString());
        var assets = body.GetProperty("assets");
        Assert.True(assets.TryGetProperty(photo.ToString(), out _));
        Assert.True(assets.TryGetProperty(listing.ToString(), out _));
        Assert.True(assets.TryGetProperty("<logo>", out _));
    }

    [Fact]
    public async Task Sends_an_image_under_its_generated_reference_when_that_reference_appears_in_a_string()
    {
        var photo = Png();
        var handler = ScriptedHandler.Json(Fixtures.SystemOne);
        using var client = TestClient.Create(handler);
        await client.SystemOneAsync(new SystemOneRequest
        {
            State = $"Compare <photo> with {photo}",
            Questions = new Dictionary<string, Question> { ["q1"] = Question.Noul("Is it the same product?") },
            Assets = new Dictionary<string, Image> { ["<photo>"] = photo },
        });

        var assets = handler.Requests[0].Body!.Value.GetProperty("assets");
        Assert.Equal("asset-0", assets.GetProperty("<photo>").GetProperty("part").GetString());
        Assert.Equal("asset-1", assets.GetProperty(photo.ToString()).GetProperty("part").GetString());
        Assert.Equal(2, handler.Requests[0].Parts.Count(p => p.Name.StartsWith("asset-")));
    }

    [Fact]
    public async Task Writes_an_image_as_its_name_when_it_is_also_in_assets()
    {
        var photo = Png();
        var listing = Png();
        var handler = ScriptedHandler.Json(Fixtures.SystemOne);
        using var client = TestClient.Create(handler);
        await client.SystemOneAsync(new SystemOneRequest
        {
            State = new Dictionary<string, object?>
            {
                ["photo"] = photo,
                ["caption"] = WithAssets.Format($"Inspect {photo} beside {listing}"),
            },
            Questions = new Dictionary<string, Question>
            {
                ["q1"] = Question.Noul(new object[] { photo }),
                ["q2"] = Question.Noul(WithAssets.Format($"Is {WithAssets.Format($"{photo}")} clear?")),
            },
            Assets = new Dictionary<string, Image> { ["<photo>"] = photo },
        });

        var body = handler.Requests[0].Body!.Value;
        Assert.Equal("<photo>", body.GetProperty("state").GetProperty("photo").GetString());
        Assert.Equal($"Inspect <photo> beside {listing}", body.GetProperty("state").GetProperty("caption").GetString());
        Assert.Equal("<photo>", body.GetProperty("questions").GetProperty("q1").GetProperty("instructions")[0].GetString());
        Assert.Equal("Is <photo> clear?", body.GetProperty("questions").GetProperty("q2").GetProperty("instructions").GetString());
        var assets = body.GetProperty("assets");
        Assert.True(assets.TryGetProperty("<photo>", out _));
        Assert.True(assets.TryGetProperty(listing.ToString(), out _));
        Assert.False(assets.TryGetProperty(photo.ToString(), out _));
    }

    [Fact]
    public async Task Rejects_unused_asset_names_before_sending()
    {
        var photo = Png();
        var handler = ScriptedHandler.Json(Fixtures.SystemOne);
        using var client = TestClient.Create(handler);
        var unused = await Assert.ThrowsAsync<TypeSafeException>(() => client.SystemOneAsync(new SystemOneRequest
        {
            State = "Inspect <photo>",
            Questions = new Dictionary<string, Question> { ["q1"] = Question.Noul("?") },
            Assets = new Dictionary<string, Image>
            {
                ["<photo>"] = photo,
                ["<unused>"] = Png(),
                ["<\"quoted\">"] = photo,
            },
        }));
        Assert.Contains("Assets not referenced in the request: <unused>, <\"quoted\">.", unused.Message);
        Assert.Contains("Write each name in `assets`", unused.Message);

        var lastNameWins = await Assert.ThrowsAsync<TypeSafeException>(() => client.SystemOneAsync(new SystemOneRequest
        {
            State = photo,
            Questions = new Dictionary<string, Question> { ["q1"] = Question.Noul("?") },
            Assets = new Dictionary<string, Image> { ["<first>"] = photo, ["<second>"] = photo },
        }));
        Assert.Contains("Assets not referenced in the request: <first>.", lastNameWins.Message);
        Assert.Empty(handler.Requests);

        await client.SystemOneAsync(new SystemOneRequest
        {
            State = "Inspect <\"quoted\">",
            Questions = new Dictionary<string, Question> { ["q1"] = Question.Noul("?") },
            Assets = new Dictionary<string, Image> { ["<\"quoted\">"] = photo },
        });
        Assert.Single(handler.Requests);
    }

    [Fact]
    public async Task Rejects_dangling_references_from_a_plain_string_before_sending()
    {
        var photo = Png();
        var handler = ScriptedHandler.Json(Fixtures.SystemOne);
        using var client = TestClient.Create(handler);
        var ex = await Assert.ThrowsAsync<TypeSafeException>(() => client.SystemOneAsync(
            $"Inspect {photo}",
            new Dictionary<string, Question> { ["q1"] = Question.Noul("?") }));
        Assert.Contains($"Image references without an attached image: {photo}.", ex.Message);
        Assert.Contains("withAssets", ex.Message);
        Assert.Empty(handler.Requests);
    }

    [Fact]
    public async Task Copies_image_bytes_at_serialize_time()
    {
        var bytes = PngBytes();
        var photo = new Image(bytes);
        var handler = ScriptedHandler.Json(Fixtures.SystemOne);
        using var client = TestClient.Create(handler);
        var pending = client.SystemOneAsync(new SystemOneRequest
        {
            State = new Dictionary<string, object?> { ["note"] = "before", ["photo"] = photo },
            Questions = new Dictionary<string, Question> { ["q1"] = Question.Noul("Is <bytes> clear?") },
            Assets = new Dictionary<string, Image> { ["<bytes>"] = new Image(bytes) },
        });
        bytes.AsSpan().Fill(0);
        await pending;

        var body = handler.Requests[0].Body!.Value;
        Assert.Equal("before", body.GetProperty("state").GetProperty("note").GetString());
        Assert.Equal(photo.ToString(), body.GetProperty("state").GetProperty("photo").GetString());
        Assert.Equal(PngBytes(), Part(handler.Requests[0], "asset-0").Bytes);
        Assert.Equal(PngBytes(), Part(handler.Requests[0], "asset-1").Bytes);
    }

    [Fact]
    public async Task Sends_join_text_and_octet_stream_for_unrecognized_magic_bytes()
    {
        var photo = Png();
        var listing = Png();
        var handler = ScriptedHandler.Json(Fixtures.SystemOne);
        using var client = TestClient.Create(handler);
        await client.SystemOneAsync(
            WithAssets.Format($"Uploads: {WithAssets.Join(new object[] { photo, listing }, ", ")}"),
            new Dictionary<string, Question>
            {
                ["q1"] = Question.Noul(WithAssets.Join(new object[] { "Is", photo, "clear?" }, " ")),
            });

        var body = handler.Requests[0].Body!.Value;
        Assert.Equal($"Uploads: {photo}, {listing}", body.GetProperty("state").GetString());
        Assert.Equal($"Is {photo} clear?", body.GetProperty("questions").GetProperty("q1").GetProperty("instructions").GetString());

        var gifHandler = ScriptedHandler.Json(Fixtures.SystemOne);
        using var gifClient = TestClient.Create(gifHandler);
        await gifClient.SystemOneAsync(new SystemOneRequest
        {
            State = "<a>",
            Questions = new Dictionary<string, Question> { ["q1"] = Question.Noul("?") },
            Assets = new Dictionary<string, Image> { ["<a>"] = new Image(Encoding.ASCII.GetBytes("GIF89a")) },
        });
        Assert.Equal("application/octet-stream", Part(gifHandler.Requests[0], "asset-0").ContentType);
    }

    [Fact]
    public async Task Multipart_puts_json_request_first_then_each_image_part()
    {
        var photo = Png();
        var jpeg = new byte[] { 0xFF, 0xD8, 0xFF };
        var handler = ScriptedHandler.Json(Fixtures.SystemOne);
        using var client = TestClient.Create(handler);
        await client.SystemOneAsync(new SystemOneRequest
        {
            State = WithAssets.Format($"Compare {photo} with <listing>"),
            Questions = new Dictionary<string, Question> { ["q1"] = Question.Noul("?") },
            Assets = new Dictionary<string, Image> { ["<listing>"] = new Image(jpeg) },
        });

        var recorded = handler.Requests[0];
        Assert.StartsWith("multipart/form-data", recorded.Headers["Content-Type"]);
        Assert.Equal(new[] { "request", "asset-0", "asset-1" }, recorded.Parts.Select(p => p.Name).ToArray());
        Assert.Equal("application/json", recorded.Parts[0].ContentType);
        Assert.True(string.IsNullOrEmpty(recorded.Parts[0].FileName));
        Assert.Equal("asset-0", recorded.Parts[1].FileName);
        Assert.Equal("image/jpeg", recorded.Parts[1].ContentType);
        Assert.Equal(jpeg, recorded.Parts[1].Bytes);
        Assert.Equal("asset-1", recorded.Parts[2].FileName);
        Assert.Equal("image/png", recorded.Parts[2].ContentType);
        Assert.Equal(PngBytes(), recorded.Parts[2].Bytes);

        var assets = recorded.Body!.Value.GetProperty("assets");
        Assert.Equal("asset-0", assets.GetProperty("<listing>").GetProperty("part").GetString());
        Assert.Equal("asset-1", assets.GetProperty(photo.ToString()).GetProperty("part").GetString());
    }

    [Fact]
    public async Task Named_like_a_generated_reference_and_from_base64_are_sent()
    {
        var name = $"<image:{new string('0', 32)}>";
        var handler = ScriptedHandler.Json(Fixtures.SystemOne);
        using var client = TestClient.Create(handler);
        await client.SystemOneAsync(new SystemOneRequest
        {
            State = $"Inspect {name}",
            Questions = new Dictionary<string, Question> { ["q1"] = Question.Noul("?") },
            Assets = new Dictionary<string, Image> { [name] = Image.FromBase64(PngBase64) },
        });
        Assert.Equal("asset-0", handler.Requests[0].Body!.Value.GetProperty("assets").GetProperty(name).GetProperty("part").GetString());
        Assert.Equal(PngBytes(), Part(handler.Requests[0], "asset-0").Bytes);
    }

    [Fact]
    public async Task Opt_in_danger_zone_header_is_sent_only_when_configured()
    {
        var handler = ScriptedHandler.Json(Fixtures.SystemOne);
        using var client = TestClient.Create(handler, o =>
        {
            o.DefaultHeaders = new Dictionary<string, string> { ["TypeSafe-Danger-Zone-Images"] = "true" };
        });
        await client.SystemOneAsync(new SystemOneRequest
        {
            State = "<a>",
            Questions = new Dictionary<string, Question> { ["q1"] = Question.Noul("?") },
            Assets = new Dictionary<string, Image> { ["<a>"] = Png() },
        });
        Assert.Equal("true", handler.Requests[0].Headers["TypeSafe-Danger-Zone-Images"]);
    }

    [Fact]
    public async Task Requests_without_images_stay_application_json()
    {
        var handler = ScriptedHandler.Json(Fixtures.SystemOne);
        using var client = TestClient.Create(handler);
        await client.SystemOneAsync("plain", new Dictionary<string, Question> { ["q1"] = Question.Noul("?") });
        Assert.Contains("application/json", handler.Requests[0].Headers["Content-Type"]);
        Assert.False(handler.Requests[0].IsMultipart);
        Assert.False(handler.Requests[0].Body!.Value.TryGetProperty("assets", out _));
    }

    private static CapturedPart Part(RecordedRequest request, string name) =>
        request.Parts.Single(p => p.Name == name);
}
