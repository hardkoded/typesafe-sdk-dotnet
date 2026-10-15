// Copyright (c) Dario Kondratiuk.
// Licensed under the MIT License.

using SkiaSharp;

namespace TypeSafe.AI.Sdk.WallySample;

/// <summary>Finds a character in a busy scene with TypeSafe box questions and highlights the answer.</summary>
internal static class WallyFinder
{
    // Box questions are meant for the jev-preview model.
    private const string Model = "jev-preview";

    // The API rejects images under about 300 pixels, so the shorter side of the zoomed view is scaled up to this size.
    private const int ZoomedSize = 512;

    /// <summary>Finds the character with two passes: a rough box in the whole scene, then a tighter one in a zoomed view of it.</summary>
    public static async Task<(BoxCoordinates Box, double? Confidence)> FindAsync(ITypeSafeClient client, string scenePath, string characterPath)
    {
        var character = Image.FromFile(characterPath);
        var roughAnswer = await AskAsync(client, Image.FromFile(scenePath), character);

        // Zoom in on the rough box, with a margin, so the small character fills more of the picture.
        var rough = Normalize(roughAnswer.Box);
        var margin = Math.Max(0.05, Math.Max(rough.Right - rough.Left, rough.Bottom - rough.Top));
        var zoom = new BoxCoordinates
        {
            Left = Math.Max(0, rough.Left - margin),
            Top = Math.Max(0, rough.Top - margin),
            Right = Math.Min(1, rough.Right + margin),
            Bottom = Math.Min(1, rough.Bottom + margin),
        };

        var zoomedAnswer = await AskAsync(client, Crop(scenePath, zoom), character);
        var tight = Normalize(zoomedAnswer.Box);
        var width = zoom.Right - zoom.Left;
        var height = zoom.Bottom - zoom.Top;
        var box = new BoxCoordinates
        {
            Left = zoom.Left + (tight.Left * width),
            Right = zoom.Left + (tight.Right * width),
            Top = zoom.Top + (tight.Top * height),
            Bottom = zoom.Top + (tight.Bottom * height),
        };

        return (Normalize(box), zoomedAnswer.Confidence);
    }

    /// <summary>Draws the box on a copy of the scene and saves it as a PNG.</summary>
    public static void Highlight(string scenePath, BoxCoordinates box, string outputPath)
    {
        using var bitmap = Decode(scenePath);
        using var canvas = new SKCanvas(bitmap);
        using var paint = new SKPaint
        {
            Color = SKColors.Magenta,
            Style = SKPaintStyle.Stroke,
            StrokeWidth = Math.Max(3, bitmap.Width / 200f),
            IsAntialias = true,
        };

        box = Normalize(box);
        canvas.DrawRect(
            (float)(box.Left * bitmap.Width),
            (float)(box.Top * bitmap.Height),
            (float)((box.Right - box.Left) * bitmap.Width),
            (float)((box.Bottom - box.Top) * bitmap.Height),
            paint);

        using var image = SKImage.FromBitmap(bitmap);
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        using var output = File.Create(outputPath);
        data.SaveTo(output);
    }

    /// <summary>Orders the corners and keeps the box inside the image, because the model can answer outside it.</summary>
    private static BoxCoordinates Normalize(BoxCoordinates box) => new()
    {
        Left = Math.Clamp(Math.Min(box.Left, box.Right), 0, 1),
        Right = Math.Clamp(Math.Max(box.Left, box.Right), 0, 1),
        Top = Math.Clamp(Math.Min(box.Top, box.Bottom), 0, 1),
        Bottom = Math.Clamp(Math.Max(box.Top, box.Bottom), 0, 1),
    };

    private static async Task<BoxAnswer> AskAsync(ITypeSafeClient client, Image scene, Image character)
    {
        var result = await client.SystemOneAsync(new SystemOneRequest
        {
            Model = Model,
            State = TextWithAssets.From($"A busy Where's Wally scene: {scene}. The character to find: {character}."),
            Questions = Question.Map(("character", Question.Box(
                TextWithAssets.From($"Where is the character from {character} in the scene {scene}? Wally wears a red and white striped shirt and bobble hat, and round glasses.")))),
        });

        return result.GetBox("character");
    }

    /// <summary>Cuts the zoom area out of the scene and scales it up, keeping its shape.</summary>
    private static Image Crop(string scenePath, BoxCoordinates area)
    {
        using var scene = Decode(scenePath);
        var source = SKRectI.Round(new SKRect(
            (float)(area.Left * scene.Width),
            (float)(area.Top * scene.Height),
            (float)(area.Right * scene.Width),
            (float)(area.Bottom * scene.Height)));
        var scale = (float)ZoomedSize / Math.Max(1, Math.Min(source.Width, source.Height));

        using var zoomed = new SKBitmap((int)(source.Width * scale), (int)(source.Height * scale));
        using var canvas = new SKCanvas(zoomed);
        using var sceneImage = SKImage.FromBitmap(scene);
        canvas.DrawImage(sceneImage, source, new SKRect(0, 0, zoomed.Width, zoomed.Height), new SKSamplingOptions(SKCubicResampler.Mitchell));

        using var image = SKImage.FromBitmap(zoomed);
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        return new Image(data.ToArray());
    }

    private static SKBitmap Decode(string path) =>
        SKBitmap.Decode(path) ?? throw new InvalidOperationException($"Cannot read '{path}' as an image. Use a PNG or JPEG file.");
}
