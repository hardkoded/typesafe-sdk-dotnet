namespace TypeSafe.AI.Sdk;

internal static class HeaderRedactor
{
    private static readonly HashSet<string> KeyHeaders = new(StringComparer.OrdinalIgnoreCase)
    {
        "authorization",
        "proxy-authorization",
        "x-api-key",
    };

    private static readonly HashSet<string> OpaqueHeaders = new(StringComparer.OrdinalIgnoreCase)
    {
        "cookie",
        "set-cookie",
    };

    public static Dictionary<string, string> Redact(IEnumerable<KeyValuePair<string, string>> headers)
    {
        var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var pair in headers)
        {
            result[pair.Key] = Redact(pair.Key, pair.Value);
        }

        return result;
    }

    private static string Redact(string name, string value)
    {
        if (KeyHeaders.Contains(name))
        {
            return RedactKey(value);
        }

        if (OpaqueHeaders.Contains(name))
        {
            return "***";
        }

        return value;
    }

    private static string RedactKey(string value)
    {
        var space = value.IndexOf(' ');
        string? scheme = null;
        var secret = value;
        if (space > 0)
        {
            scheme = value.Substring(0, space);
            secret = value.Substring(space + 1).TrimStart();
        }

        var tail = secret.Length > 8 ? secret.Substring(secret.Length - 4) : "";
        return scheme is null ? $"***{tail}" : $"{scheme} ***{tail}";
    }
}
