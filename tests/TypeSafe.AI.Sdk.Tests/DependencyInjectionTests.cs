using Microsoft.Extensions.DependencyInjection;
using TypeSafe.AI.Sdk.DependencyInjection;
using Xunit;

namespace TypeSafe.AI.Sdk.Tests;

public sealed class DependencyInjectionTests
{
    [Fact]
    public void AddTypeSafeClient_registers_factory_backed_client()
    {
        var services = new ServiceCollection();
        services.AddTypeSafeClient(o =>
        {
            o.ApiKey = "di-key";
            o.BaseUrl = "https://di.test";
            o.LogLevel = TypeSafeLogLevel.Off;
        });

        using var provider = services.BuildServiceProvider();
        var client = provider.GetRequiredService<ITypeSafeClient>();
        var typed = provider.GetRequiredService<TypeSafeClient>();
        Assert.Equal("https://di.test", client.BaseUrl);
        Assert.Equal("https://di.test", typed.BaseUrl);
    }
}
