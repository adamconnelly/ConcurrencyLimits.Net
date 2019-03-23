# ASP.NET Core

This spec contains a description of how the library will be used in ASP.NET Core.

## Configuration

Configuration will be done in the `Startup.cs` file for the application, in the
`ConfigureServices()` method, for example:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddConcurrencyLimits()
        .UseSimpleLimiter()
        .WithFixedLimit(5);
}
```

### Beginning Configuration

To start the configuration process, we can call the `AddConcurrencyLimits()`
extension method. It has the following signature:

```csharp
public static ILimiterConfigurer AddConcurrencyLimits(this IServiceCollection services)
{
}
```

### Choosing a Limiter

The `IConfigureLimiter` interface allows the limiter to be selected:

```csharp
public interface ILimiterConfigurer
{
    IConfigureLimit UseSimpleLimiter();
    IConfigureLimit UseSomeOtherLimiter();
    ...
}
```

We'll add a method for each type of limiter that can be configured.

### Choosing the Limit

The `ILimitConfigurer` interface allows the limit to be specified:

```csharp
public interface ILimitConfigurer
{
    void WithFixedLimit(int limit);
    void WithAdaptiveLimit(int initialLimit, ...);
}
```

## Enabling the Middleware

To enable the middleware, you can call the `UseConcurrencyLimits()` extension
method when configuring the pipeline in the `Configure()` method:

```csharp
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    app.UseConcurrencyLimits();
    app.UseMvc();
}
```

You'll want to add the limiter quite early in the pipeline to avoid doing
unnecessary work for requests that will end up being limited anyway.
