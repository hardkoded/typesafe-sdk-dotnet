// Copyright (c) Dario Kondratiuk.
// Licensed under the MIT License.

using System.Collections;
using System.Globalization;
using System.Text.RegularExpressions;

namespace TypeSafe.AI.Sdk;

/// <summary>Text with the images interpolated into it, sent together with the text.</summary>
public sealed class TextWithAssets
{
    private TextWithAssets(string text, IReadOnlyList<Image> assets)
    {
        Text = text;
        Assets = assets;
    }

    /// <summary>The text, with each image replaced by its reference.</summary>
    public string Text { get; }

    /// <summary>The images interpolated into the text, including those in nested <see cref="TextWithAssets"/>.</summary>
    public IReadOnlyList<Image> Assets { get; }

    /// <summary>
    /// Write images into text: <c>TextWithAssets.From($"Is {photo} damaged?")</c>.
    /// Each image is replaced by its reference; other values are converted to strings as in an interpolated string.
    /// An image in a plain interpolated string is rejected before the request is sent.
    /// </summary>
    public static TextWithAssets From(FormattableString text)
    {
        return Create(text.ToString(CultureInfo.InvariantCulture), text.GetArguments());
    }

    /// <summary>
    /// Join <paramref name="values"/> with <paramref name="separator"/>, as <see cref="string.Join(string, IEnumerable{string})"/>
    /// does, and send the images among them with the text. The result can be passed as content or interpolated into
    /// <see cref="From(FormattableString)"/>.
    /// </summary>
    public static TextWithAssets Join(IEnumerable<object?> values, string separator = ",")
    {
        var list = values.ToList();
        return Create(string.Join(separator, list.Select(Stringify)), list);
    }

    /// <inheritdoc />
    public override string ToString() => Text;

    /// <summary>A value as text; a nested sequence is joined with commas, so the images in it are written too.</summary>
    private static string? Stringify(object? value) =>
        value is IEnumerable items and not string ? string.Join(",", items.Cast<object?>().Select(Stringify)) : value?.ToString();

    private static TextWithAssets Create(string text, IReadOnlyList<object?> values)
    {
        var interpolated = new Dictionary<string, Image>(StringComparer.Ordinal);
        Collect(values, interpolated, new HashSet<object>());

        // Only images whose references the text contains, in order of appearance.
        var assets = new Dictionary<string, Image>(StringComparer.Ordinal);
        foreach (Match match in AssetContext.Reference.Matches(text))
        {
            if (interpolated.TryGetValue(match.Value, out var image))
            {
                assets[match.Value] = image;
            }
        }

        return new TextWithAssets(text, assets.Values.ToList());
    }

    private static void Collect(object? value, Dictionary<string, Image> found, HashSet<object> visited)
    {
        switch (value)
        {
            case null or string:
                return;
            case Image image:
                found[image.ToString()] = image;
                return;
            case TextWithAssets text:
                foreach (var asset in text.Assets)
                {
                    found[asset.ToString()] = asset;
                }

                return;
            case IEnumerable items when visited.Add(items):
                foreach (var item in items)
                {
                    Collect(item, found, visited);
                }

                return;
        }
    }
}
