// Copyright (c) Dario Kondratiuk.
// Licensed under the MIT License.

using System.Collections;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace TypeSafe.AI.Sdk;

/// <summary>
/// Text from <see cref="WithAssets"/>, with the assets interpolated into it.
/// Mirrors JS <c>TextWithAssets</c>.
/// </summary>
[JsonConverter(typeof(TextWithAssetsJsonConverter))]
public sealed class TextWithAssets
{
    /// <summary>The text, with each asset replaced by its reference.</summary>
    public string Text { get; }

    /// <summary>
    /// The assets interpolated into the text, including those in nested <see cref="WithAssets"/> text,
    /// in order of appearance.
    /// </summary>
    public IReadOnlyList<Image> Assets { get; }

    /// <summary>Create text that already has assets interpolated as references.</summary>
    public TextWithAssets(string text, IReadOnlyList<Image> assets)
    {
        Text = text ?? "";
        Assets = assets ?? Array.Empty<Image>();
    }

    /// <summary>The text, written wherever this value is converted to a string.</summary>
    public override string ToString() => Text;
}

/// <summary>
/// Interpolate images into text and attach them to the request.
/// Mirrors JS <c>withAssets</c> tagged templates and <c>withAssets.join</c>.
/// </summary>
/// <remarks>
/// C# has no tagged templates. Use <see cref="Format(FormattableString)"/> with an interpolated
/// string, <see cref="Format(string, object?[])"/> with composite format items, or
/// <see cref="Join"/>. An image interpolated into a plain string (not <see cref="WithAssets"/>)
/// is not attached; the request is rejected before send.
/// </remarks>
public static class WithAssets
{
    /// <summary>
    /// Format an interpolated string so the assets interpolated into it are sent with the text.
    /// Each asset is replaced by its reference; other values are converted to strings.
    /// Mirrors JS <c>withAssets`...`</c>.
    /// </summary>
    /// <example>
    /// <code>
    /// var state = WithAssets.Format($"Compare {photo} with the listing.");
    /// </code>
    /// </example>
    public static TextWithAssets Format(FormattableString formattable)
    {
        if (formattable is null)
        {
            throw new TypeSafeException("Format string must not be null.");
        }

        return FromText(formattable.ToString(CultureInfo.InvariantCulture), formattable.GetArguments());
    }

    /// <summary>
    /// Format a composite format string so the assets among <paramref name="args"/> are sent with the text.
    /// Mirrors JS <c>withAssets`...`</c> when values are passed as format arguments.
    /// </summary>
    /// <example>
    /// <code>
    /// var state = WithAssets.Format("Compare {0} with {1}.", photo, listing);
    /// </code>
    /// </example>
    public static TextWithAssets Format(string format, params object?[] args)
    {
        if (format is null)
        {
            throw new TypeSafeException("Format string must not be null.");
        }

        args ??= Array.Empty<object?>();
        return FromText(string.Format(CultureInfo.InvariantCulture, format, args), args);
    }

    /// <summary>
    /// Join <paramref name="values"/> with <paramref name="separator"/> and send the assets among
    /// them with the text. The result can be passed as content or interpolated into
    /// <see cref="Format(FormattableString)"/>.
    /// Mirrors JS <c>withAssets.join</c>.
    /// </summary>
    /// <param name="values">Values to join. Images are written as their references.</param>
    /// <param name="separator">Written between values. Default: <c>,</c>.</param>
    /// <example>
    /// <code>
    /// var state = WithAssets.Format($"The seller uploaded {WithAssets.Join(uploads, ", ")}.");
    /// </code>
    /// </example>
    public static TextWithAssets Join(IEnumerable<object?> values, string separator = ",")
    {
        if (values is null)
        {
            throw new TypeSafeException("Values to join must not be null.");
        }

        var list = values as IList<object?> ?? new List<object?>(values);
        var pieces = new string[list.Count];
        for (var i = 0; i < list.Count; i++)
        {
            pieces[i] = list[i]?.ToString() ?? "";
        }

        return FromText(string.Join(separator ?? ",", pieces), list);
    }

    internal static TextWithAssets FromText(string text, IEnumerable<object?> values)
    {
        var interpolated = new Dictionary<string, Image>(StringComparer.Ordinal);
        var visited = new HashSet<object>(ReferenceEqualityComparer.Instance);
        foreach (var value in values)
        {
            CollectAssets(value, interpolated, visited);
        }

        var assets = new List<Image>();
        foreach (Match match in Image.s_referencePattern.Matches(text))
        {
            if (interpolated.TryGetValue(match.Value, out var asset))
            {
                interpolated.Remove(match.Value);
                assets.Add(asset);
            }
        }

        return new TextWithAssets(text, assets);
    }

    /// <summary>Add the assets in <paramref name="value"/>, including those in nested text, to <paramref name="found"/> by reference.</summary>
    internal static void CollectAssets(object? value, Dictionary<string, Image> found, HashSet<object> visited)
    {
        if (value is null || value is string)
        {
            return;
        }

        var type = value.GetType();
        if (type.IsPrimitive || value is decimal)
        {
            return;
        }

        if (!visited.Add(value))
        {
            return;
        }

        switch (value)
        {
            case Image image:
                found[image.ToString()] = image;
                return;
            case TextWithAssets text:
                foreach (var asset in text.Assets)
                {
                    found[asset.ToString()] = asset;
                }

                return;
            case IDictionary<string, object?> map:
                foreach (var pair in map)
                {
                    CollectAssets(pair.Value, found, visited);
                }

                return;
            case IDictionary map:
                foreach (DictionaryEntry entry in map)
                {
                    CollectAssets(entry.Value, found, visited);
                }

                return;
            case IEnumerable enumerable:
                foreach (var item in enumerable)
                {
                    CollectAssets(item, found, visited);
                }

                return;
        }
    }

}

internal sealed class TextWithAssetsJsonConverter : JsonConverter<TextWithAssets>
{
    private readonly AssetWriteContext? _context;

    public TextWithAssetsJsonConverter()
        : this(null)
    {
    }

    public TextWithAssetsJsonConverter(AssetWriteContext? context)
    {
        _context = context;
    }

    public override TextWithAssets Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotSupportedException("TextWithAssets values cannot be deserialized from JSON.");
    }

    public override void Write(Utf8JsonWriter writer, TextWithAssets value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(_context is null ? value.Text : _context.WriteText(value));
    }
}

internal sealed class ReferenceEqualityComparer : IEqualityComparer<object>
{
    public static readonly ReferenceEqualityComparer Instance = new();

    public new bool Equals(object? x, object? y) => ReferenceEquals(x, y);

    public int GetHashCode(object obj) => RuntimeHelpers.GetHashCode(obj);
}
