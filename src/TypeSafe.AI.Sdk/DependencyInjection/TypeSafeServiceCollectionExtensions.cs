// Copyright (c) Dario Kondratiuk.
// Licensed under the MIT License.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace TypeSafe.AI.Sdk.DependencyInjection;

/// <summary>Dependency-injection helpers for <see cref="TypeSafeClient"/>.</summary>
public static class TypeSafeServiceCollectionExtensions
{
    /// <summary>
    /// Register <see cref="ITypeSafeClient"/> / <see cref="TypeSafeClient"/> using
    /// <see cref="IHttpClientFactory"/>. Configuration still reads <c>TYPESAFE_API_KEY</c>
    /// and the other <see cref="Env"/> variables when options omit them.
    /// </summary>
    public static IHttpClientBuilder AddTypeSafeClient(
        this IServiceCollection services,
        Action<TypeSafeClientOptions>? configure = null)
    {
#if NET
        ArgumentNullException.ThrowIfNull(services);
#else
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }
#endif

        if (configure is not null)
        {
            services.Configure(configure);
        }
        else
        {
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<TypeSafeClientOptions>, UnconfiguredTypeSafeClientOptions>());
        }

        services.TryAddSingleton<ITypeSafeLogger>(sp =>
        {
            var logger = sp.GetService<ILogger<TypeSafeClient>>();
            return logger is null ? new ConsoleTypeSafeLogger() : new MicrosoftLoggingTypeSafeLogger(logger);
        });

        var builder = services.AddHttpClient(TypeSafeClient.HttpClientName, client =>
        {
            client.Timeout = Timeout.InfiniteTimeSpan;
        });

        services.TryAddTransient(CreateClient);
        services.TryAddTransient<ITypeSafeClient>(sp => sp.GetRequiredService<TypeSafeClient>());
        return builder;
    }

    private static TypeSafeClient CreateClient(IServiceProvider sp)
    {
        var factory = sp.GetRequiredService<IHttpClientFactory>();
        var http = factory.CreateClient(TypeSafeClient.HttpClientName);
        var options = sp.GetService<IOptions<TypeSafeClientOptions>>()?.Value ?? new TypeSafeClientOptions();
        if (options.Logger is null)
        {
            options.Logger = sp.GetService<ITypeSafeLogger>();
        }

        return new TypeSafeClient(http, options);
    }

    private sealed class UnconfiguredTypeSafeClientOptions : IConfigureOptions<TypeSafeClientOptions>
    {
        public void Configure(TypeSafeClientOptions options)
        {
            // Environment variables are read by TypeSafeClient itself.
        }
    }
}
