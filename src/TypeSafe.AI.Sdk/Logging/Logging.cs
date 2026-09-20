namespace TypeSafe.AI.Sdk;

/// <summary>Log verbosity; <see cref="Off"/> disables logging. Mirrors JS <c>LogLevel</c>.</summary>
public enum TypeSafeLogLevel
{
    /// <summary>Headers and request/response bodies.</summary>
    Debug = 0,

    /// <summary>Request summaries and retry decisions.</summary>
    Info = 1,

    /// <summary>Warnings. Default.</summary>
    Warn = 2,

    /// <summary>Errors only.</summary>
    Error = 3,

    /// <summary>Disable logging.</summary>
    Off = 4,
}

/// <summary>Supported log level names, from most to least verbose.</summary>
public static class LogLevels
{
    /// <summary>Canonical names matching the JS SDK: debug, info, warn, error, off.</summary>
    public static readonly IReadOnlyList<string> All = new[] { "debug", "info", "warn", "error", "off" };

    /// <summary>Default log level (<c>warn</c>).</summary>
    public const TypeSafeLogLevel Default = TypeSafeLogLevel.Warn;

    /// <summary>Validate a configured log level, throwing <see cref="TypeSafeException"/> for unknown values.</summary>
    public static TypeSafeLogLevel Parse(string value, string source)
    {
        switch (value.Trim().ToLowerInvariant())
        {
            case "debug": return TypeSafeLogLevel.Debug;
            case "info": return TypeSafeLogLevel.Info;
            case "warn":
            case "warning": return TypeSafeLogLevel.Warn;
            case "error": return TypeSafeLogLevel.Error;
            case "off":
            case "none": return TypeSafeLogLevel.Off;
            default:
                throw new TypeSafeException(
                    $"Invalid log level \"{value}\" from {source}. Expected one of: {string.Join(", ", All)}.");
        }
    }

    /// <summary>JS-style name for a level.</summary>
    public static string ToJsName(TypeSafeLogLevel level) =>
        level switch
        {
            TypeSafeLogLevel.Debug => "debug",
            TypeSafeLogLevel.Info => "info",
            TypeSafeLogLevel.Warn => "warn",
            TypeSafeLogLevel.Error => "error",
            TypeSafeLogLevel.Off => "off",
            _ => "warn",
        };
}

/// <summary>Log methods accepting a message and structured values. Compatible with <c>console</c> in spirit.</summary>
public interface ITypeSafeLogger
{
    /// <summary>Write a debug message.</summary>
    void Debug(string message, params object?[] args);

    /// <summary>Write an informational message.</summary>
    void Info(string message, params object?[] args);

    /// <summary>Write a warning.</summary>
    void Warn(string message, params object?[] args);

    /// <summary>Write an error message.</summary>
    void Error(string message, params object?[] args);
}

/// <summary>Default console logger with the <c>[typesafe-sdk]</c> prefix.</summary>
public sealed class ConsoleTypeSafeLogger : ITypeSafeLogger
{
    private const string Prefix = "[typesafe-sdk]";

    /// <inheritdoc />
    public void Debug(string message, params object?[] args) =>
        Console.Out.WriteLine(Format("debug", message, args));

    /// <inheritdoc />
    public void Info(string message, params object?[] args) =>
        Console.Out.WriteLine(Format("info", message, args));

    /// <inheritdoc />
    public void Warn(string message, params object?[] args) =>
        Console.Error.WriteLine(Format("warn", message, args));

    /// <inheritdoc />
    public void Error(string message, params object?[] args) =>
        Console.Error.WriteLine(Format("error", message, args));

    private static string Format(string level, string message, object?[] args)
    {
        if (args.Length == 0)
        {
            return $"{Prefix} {message}";
        }

        try
        {
            return $"{Prefix} {message} {string.Join(" ", args.Select(a => a is null ? "null" : a.ToString()))}";
        }
        catch
        {
            return $"{Prefix} {message}";
        }
    }
}

/// <summary>Adapts <see cref="Microsoft.Extensions.Logging.ILogger"/> to <see cref="ITypeSafeLogger"/>.</summary>
public sealed class MicrosoftLoggingTypeSafeLogger : ITypeSafeLogger
{
    private readonly Microsoft.Extensions.Logging.ILogger _logger;

    /// <summary>Create an adapter around a Microsoft logger.</summary>
    public MicrosoftLoggingTypeSafeLogger(Microsoft.Extensions.Logging.ILogger logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public void Debug(string message, params object?[] args) =>
        _logger.Log(Microsoft.Extensions.Logging.LogLevel.Debug, message, args);

    /// <inheritdoc />
    public void Info(string message, params object?[] args) =>
        _logger.Log(Microsoft.Extensions.Logging.LogLevel.Information, message, args);

    /// <inheritdoc />
    public void Warn(string message, params object?[] args) =>
        _logger.Log(Microsoft.Extensions.Logging.LogLevel.Warning, message, args);

    /// <inheritdoc />
    public void Error(string message, params object?[] args) =>
        _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, message, args);
}

internal sealed class LevelFilteredLogger : ITypeSafeLogger
{
    private readonly ITypeSafeLogger _inner;
    private readonly TypeSafeLogLevel _level;

    public LevelFilteredLogger(ITypeSafeLogger inner, TypeSafeLogLevel level)
    {
        _inner = inner;
        _level = level;
    }

    public void Debug(string message, params object?[] args)
    {
        if (_level <= TypeSafeLogLevel.Debug) _inner.Debug(message, args);
    }

    public void Info(string message, params object?[] args)
    {
        if (_level <= TypeSafeLogLevel.Info) _inner.Info(message, args);
    }

    public void Warn(string message, params object?[] args)
    {
        if (_level <= TypeSafeLogLevel.Warn) _inner.Warn(message, args);
    }

    public void Error(string message, params object?[] args)
    {
        if (_level <= TypeSafeLogLevel.Error) _inner.Error(message, args);
    }
}

internal static class MicrosoftLoggerExtensions
{
    public static void Log(this Microsoft.Extensions.Logging.ILogger logger, Microsoft.Extensions.Logging.LogLevel level, string message, object?[] args)
    {
        if (args.Length == 0)
        {
            logger.Log(level, 0, message, null, (state, _) => state);
            return;
        }

        logger.Log(level, 0, (message, args), null, static (state, _) =>
        {
            try
            {
                return $"{state.message} {string.Join(" ", state.args.Select(a => a is null ? "null" : a.ToString()))}";
            }
            catch
            {
                return state.message;
            }
        });
    }
}
