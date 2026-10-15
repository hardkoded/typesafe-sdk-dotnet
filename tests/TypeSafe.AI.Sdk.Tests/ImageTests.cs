// Copyright (c) Dario Kondratiuk.
// Licensed under the MIT License.

using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace TypeSafe.AI.Sdk.Tests;

public sealed class ImageTests
{
    private static readonly byte[] s_pngBytes = { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A, 1, 2, 3, 0xFF };
    private static readonly byte[] s_jpegBytes = { 0xFF, 0xD8, 0xFF, 0xE0, 9, 8, 7 };

    private static Dictionary<string, Question> Ask(object? instructions = null) =>
        new() { ["q1"] = Question.Noul(instructions ?? "Is it damaged?") };

    [Fact]
    public void Image_rejects_empty_data_and_bad_base64()
    {
        Assert.Throws<TypeSafeException>(() => new Image(ReadOnlyMemory<byte>.Empty));
        Assert.Throws<TypeSafeException>(() => Image.FromBase64(""));
        var ex = Assert.Throws<TypeSafeException>(() => Image.FromBase64("not base64!"));
        Assert.Contains("valid base64", ex.Message);
    }

    [Fact]
    public void Image_copies_its_bytes_and_has_a_unique_reference()
    {
        var bytes = (byte[])s_pngBytes.Clone();
        var one = new Image(bytes);
        bytes[0] = 0;
        Assert.Equal(s_pngBytes, one.Data.ToArray());
        Assert.Equal(s_pngBytes, Image.FromBase64(Convert.ToBase64String(s_pngBytes)).Data.ToArray());
        Assert.Matches("^<image:[0-9a-f]{32}>$", one.ToString());
        Assert.NotEqual(one.ToString(), new Image(bytes).ToString());
    }

    [Fact]
    public async Task Box_question_with_an_image_state_is_sent_as_multipart()
    {
        var handler = ScriptedHandler.Json(Fixtures.SystemOne);
        using var client = TestClient.Create(handler);
        var photo = new Image(s_pngBytes);

        await client.SystemOneAsync(new SystemOneRequest
        {
            State = photo,
            Questions = Question.Map(("damage", Question.Box("The damaged area"))),
            Model = "jev-preview",
        });

        var form = Multipart.Parse(handler.Requests[0]);
        Assert.Equal(new[] { "request", "asset-0" }, form.Select(p => p.Name));
        var json = JsonDocument.Parse(form[0].Text).RootElement;
        Assert.Equal("jev-preview", json.GetProperty("model").GetString());
        Assert.Equal(photo.ToString(), json.GetProperty("state").GetString());
        Assert.Equal("box", json.GetProperty("questions").GetProperty("damage").GetProperty("type").GetString());
    }

    [Fact]
    public async Task Image_in_state_is_sent_as_multipart_under_its_reference()
    {
        var handler = ScriptedHandler.Json(Fixtures.SystemOne);
        using var client = TestClient.Create(handler, o => o.DefaultHeaders = new Dictionary<string, string>
        {
            ["TypeSafe-Danger-Zone-Images"] = "true",
        });
        var photo = new Image(s_pngBytes);

        await client.SystemOneAsync(new Dictionary<string, object?> { ["claim"] = "chipped", ["photo"] = photo }, Ask());

        var request = handler.Requests[0];
        Assert.Equal("true", request.Headers["TypeSafe-Danger-Zone-Images"]);
        var form = Multipart.Parse(request);
        Assert.Equal(new[] { "request", "asset-0" }, form.Select(p => p.Name));
        var json = JsonDocument.Parse(form[0].Text).RootElement;
        Assert.Equal(photo.ToString(), json.GetProperty("state").GetProperty("photo").GetString());
        var asset = json.GetProperty("assets").GetProperty(photo.ToString());
        Assert.Equal("image", asset.GetProperty("type").GetString());
        Assert.Equal("asset-0", asset.GetProperty("part").GetString());
        Assert.Equal("image/png", form[1].ContentType);
        Assert.Equal(s_pngBytes, form[1].Bytes);
    }

    [Fact]
    public async Task Image_inside_an_anonymous_object_is_sent_under_its_reference()
    {
        var handler = ScriptedHandler.Json(Fixtures.SystemOne);
        using var client = TestClient.Create(handler);
        var photo = new Image(s_pngBytes);

        await client.SystemOneAsync(new { claim = "chipped", photo }, Ask());

        var form = Multipart.Parse(handler.Requests[0]);
        Assert.Equal(2, form.Count);
        var json = JsonDocument.Parse(form[0].Text).RootElement;
        Assert.Equal(photo.ToString(), json.GetProperty("state").GetProperty("photo").GetString());
    }

    [Fact]
    public async Task Multipart_boundary_is_not_quoted()
    {
        var handler = ScriptedHandler.Json(Fixtures.SystemOne);
        using var client = TestClient.Create(handler);

        await client.SystemOneAsync(new Image(s_jpegBytes), Ask());

        Assert.Matches("^multipart/form-data; boundary=[0-9a-f]{32}$", handler.Requests[0].Headers["Content-Type"]);
        Assert.Equal("image/jpeg", Multipart.Parse(handler.Requests[0])[1].ContentType);
    }

    [Fact]
    public async Task Unknown_formats_are_sent_as_octet_stream()
    {
        var handler = ScriptedHandler.Json(Fixtures.SystemOne);
        using var client = TestClient.Create(handler);

        await client.SystemOneAsync(new Image(new byte[] { 1, 2, 3 }), Ask());

        Assert.Equal("application/octet-stream", Multipart.Parse(handler.Requests[0])[1].ContentType);
    }

    [Fact]
    public async Task Requests_without_images_stay_json()
    {
        var handler = ScriptedHandler.Json(Fixtures.SystemOne);
        using var client = TestClient.Create(handler);

        await client.SystemOneAsync("text", Ask());

        Assert.StartsWith("application/json", handler.Requests[0].Headers["Content-Type"]);
        Assert.False(handler.Requests[0].Body!.Value.TryGetProperty("assets", out _));
    }

    [Fact]
    public async Task WithAssets_writes_images_into_text_and_sends_each_image_once()
    {
        var handler = ScriptedHandler.Json(Fixtures.SystemOne);
        using var client = TestClient.Create(handler);
        var upload = new Image(s_pngBytes);
        var catalog = new Image(s_jpegBytes);

        await client.SystemOneAsync(
            TextWithAssets.From($"A seller uploaded {upload} for a listing whose photo is {catalog}."),
            Ask(TextWithAssets.From($"Does {upload} show the same product as {catalog}?")));

        var form = Multipart.Parse(handler.Requests[0]);
        Assert.Equal(3, form.Count);
        var json = JsonDocument.Parse(form[0].Text).RootElement;
        Assert.Equal(
            $"A seller uploaded {upload} for a listing whose photo is {catalog}.",
            json.GetProperty("state").GetString());
        Assert.Equal(
            $"Does {upload} show the same product as {catalog}?",
            json.GetProperty("questions").GetProperty("q1").GetProperty("instructions").GetString());
        Assert.Equal(2, json.GetProperty("assets").EnumerateObject().Count());
        Assert.Equal(s_pngBytes, form[1].Bytes);
        Assert.Equal(s_jpegBytes, form[2].Bytes);
    }

    [Fact]
    public async Task WithAssets_Join_and_nesting_carry_their_images()
    {
        var handler = ScriptedHandler.Json(Fixtures.SystemOne);
        using var client = TestClient.Create(handler);
        var one = new Image(s_pngBytes);
        var two = new Image(s_jpegBytes);

        var list = TextWithAssets.Join(new object[] { one, two }, ", ");
        Assert.Equal($"{one}, {two}", list.Text);
        Assert.Equal(2, list.Assets.Count);

        await client.SystemOneAsync(TextWithAssets.From($"The seller uploaded {list}."), Ask());

        var form = Multipart.Parse(handler.Requests[0]);
        Assert.Equal(new[] { "request", "asset-0", "asset-1" }, form.Select(p => p.Name));
    }

    [Fact]
    public async Task Images_in_nested_lists_and_dictionaries_are_found()
    {
        var handler = ScriptedHandler.Json(Fixtures.SystemOne);
        using var client = TestClient.Create(handler);
        var one = new Image(s_pngBytes);
        var two = new Image(s_jpegBytes);
        var questions = new Dictionary<string, Question>
        {
            ["q1"] = Question.Choice("Which?", ("a", one), ("b", null)),
            ["q2"] = Question.Score(new object[] { "How bad?", two }, "low", "high"),
        };

        await client.SystemOneAsync(new object[] { new Dictionary<string, object?> { ["x"] = one } }, questions);

        Assert.Equal(3, Multipart.Parse(handler.Requests[0]).Count);
    }

    [Fact]
    public async Task Named_assets_replace_references_in_content_and_text()
    {
        var handler = ScriptedHandler.Json(Fixtures.SystemOne);
        using var client = TestClient.Create(handler);
        var delivered = new Image(s_pngBytes);
        var catalog = new Image(s_jpegBytes);

        await client.SystemOneAsync(new SystemOneRequest
        {
            State = "A customer returned <delivered>. The catalog photo is <catalog>.",
            Questions = Ask(TextWithAssets.From($"Is {delivered} the product shown in <catalog>?")),
            Assets = new Dictionary<string, Image> { ["<delivered>"] = delivered, ["<catalog>"] = catalog },
        });

        var form = Multipart.Parse(handler.Requests[0]);
        var json = JsonDocument.Parse(form[0].Text).RootElement;
        Assert.Equal(
            "Is <delivered> the product shown in <catalog>?",
            json.GetProperty("questions").GetProperty("q1").GetProperty("instructions").GetString());
        var assets = json.GetProperty("assets");
        Assert.Equal("asset-0", assets.GetProperty("<delivered>").GetProperty("part").GetString());
        Assert.Equal("asset-1", assets.GetProperty("<catalog>").GetProperty("part").GetString());
        Assert.DoesNotContain("<image:", form[0].Text);
        Assert.Equal(s_pngBytes, form[1].Bytes);
        Assert.Equal(s_jpegBytes, form[2].Bytes);
    }

    [Fact]
    public async Task A_named_image_placed_in_content_is_written_as_its_name()
    {
        var handler = ScriptedHandler.Json(Fixtures.SystemOne);
        using var client = TestClient.Create(handler);
        var photo = new Image(s_pngBytes);

        await client.SystemOneAsync(new SystemOneRequest
        {
            State = photo,
            Questions = Ask(),
            Assets = new Dictionary<string, Image> { ["<photo>"] = photo },
        });

        var json = JsonDocument.Parse(Multipart.Parse(handler.Requests[0])[0].Text).RootElement;
        Assert.Equal("<photo>", json.GetProperty("state").GetString());
    }

    [Fact]
    public async Task Rejects_a_named_asset_that_the_request_never_mentions()
    {
        var handler = ScriptedHandler.Json(Fixtures.SystemOne);
        using var client = TestClient.Create(handler);

        var ex = await Assert.ThrowsAsync<TypeSafeException>(() => client.SystemOneAsync(new SystemOneRequest
        {
            State = "nothing here",
            Questions = Ask(),
            Assets = new Dictionary<string, Image> { ["<photo>"] = new Image(s_pngBytes) },
        }));

        Assert.Contains("Assets not referenced in the request: <photo>", ex.Message);
        Assert.Empty(handler.Requests);
    }

    [Fact]
    public async Task Rejects_an_image_in_a_plain_interpolated_string()
    {
        var handler = ScriptedHandler.Json(Fixtures.SystemOne);
        using var client = TestClient.Create(handler);
        var photo = new Image(s_pngBytes);

        var ex = await Assert.ThrowsAsync<TypeSafeException>(() => client.SystemOneAsync($"Compare {photo}", Ask()));

        Assert.Contains("Image references without an attached image", ex.Message);
        Assert.Contains(photo.ToString(), ex.Message);
        Assert.Empty(handler.Requests);
    }

    [Fact]
    public async Task Rejects_a_named_image_in_a_plain_interpolated_string()
    {
        var handler = ScriptedHandler.Json(Fixtures.SystemOne);
        using var client = TestClient.Create(handler);
        var photo = new Image(s_pngBytes);

        var ex = await Assert.ThrowsAsync<TypeSafeException>(() => client.SystemOneAsync(new SystemOneRequest
        {
            State = $"see {photo} and <photo>",
            Questions = Ask(),
            Assets = new Dictionary<string, Image> { ["<photo>"] = photo },
        }));

        Assert.Contains("Image references without an attached image", ex.Message);
    }

    [Fact]
    public async Task Resends_the_whole_multipart_body_on_retry()
    {
        var handler = ScriptedHandler.Sequence(
            ScriptedHandler.JsonResponse(new { error = "busy" }, HttpStatusCode.InternalServerError),
            ScriptedHandler.JsonResponse(Fixtures.SystemOne));
        using var client = TestClient.Create(handler, o => o.Retry = new RetryPolicy { BackoffInitialMs = 0, BackoffJitter = 0 });

        await client.SystemOneAsync(new Image(s_pngBytes), Ask());

        Assert.Equal(2, handler.Requests.Count);
        Assert.Equal(s_pngBytes, Multipart.Parse(handler.Requests[1])[1].Bytes);
    }

    private sealed record Part(string Name, string ContentType, byte[] Bytes)
    {
        public string Text => Encoding.UTF8.GetString(Bytes);
    }

    private static class Multipart
    {
        public static List<Part> Parse(RecordedRequest request)
        {
            var contentType = request.Headers["Content-Type"];
            Assert.StartsWith("multipart/form-data; boundary=", contentType);
            var boundary = "--" + contentType["multipart/form-data; boundary=".Length..];

            // Latin-1 maps every byte to one char, so offsets in the text are offsets in the bytes.
            var bytes = request.Headers.GetValueOrDefault("Content-Encoding") == "zstd"
                ? Decompress(request.BodyBytes!)
                : request.BodyBytes!;
            var raw = Encoding.Latin1.GetString(bytes);
            var parts = new List<Part>();
            foreach (var chunk in raw.Split(boundary).Skip(1))
            {
                if (chunk.StartsWith("--", StringComparison.Ordinal))
                {
                    break;
                }

                var split = chunk.IndexOf("\r\n\r\n", StringComparison.Ordinal);
                var headers = chunk[..split];
                var body = chunk[(split + 4)..^2];
                var name = System.Text.RegularExpressions.Regex.Match(headers, "name=\"?([^\";\\r\\n]+)").Groups[1].Value;
                var type = System.Text.RegularExpressions.Regex.Match(headers, "Content-Type: ([^\\r\\n]+)").Groups[1].Value;
                parts.Add(new Part(name, type.Split(';')[0], Encoding.Latin1.GetBytes(body)));
            }

            return parts;
        }

        private static byte[] Decompress(byte[] bytes)
        {
            using var decompressor = new ZstdSharp.Decompressor();
            return decompressor.Unwrap(bytes).ToArray();
        }
    }
}
