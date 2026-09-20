# Dependency injection

Register the client with `IServiceCollection`. Uses `IHttpClientFactory` and the named client `TypeSafe.AI`.

```csharp
services.AddTypeSafeClient(options =>
{
    options.ApiKey = builder.Configuration["TYPESAFE_API_KEY"];
});

public sealed class Tickets(ITypeSafeClient typesafe)
{
    // inject and call SystemOneAsync
}
```

See <xref:TypeSafe.AI.Sdk.DependencyInjection.TypeSafeServiceCollectionExtensions> and <xref:TypeSafe.AI.Sdk.ITypeSafeClient>.
