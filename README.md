# EazyHttp
Friendly and helpful library to handle HTTP requests in .NET projects
=====================================================================

------------------------------------------------------------------
Source: [GitHub repo](https://github.com/ceccomil/eazy-http)

Packages
--------
| Package | NuGet Stable | NuGet Pre-release | Downloads |
| ------- | ------------ | ----------------- | --------- | 
| [EazyHttp](https://www.nuget.org/packages/EazyHttp) | [![EazyHttp](https://img.shields.io/nuget/v/EazyHttp.svg)](https://www.nuget.org/packages/EazyHttp) | [![EazyHttp](https://img.shields.io/nuget/vpre/EazyHttp.svg)](https://www.nuget.org/packages/EazyHttp/) | [![EazyHttp](https://img.shields.io/nuget/dt/EazyHttp.svg)](https://www.nuget.org/packages/EazyHttp/) |

Features
--------
- Typed HTTP client wrappers generated at compile time
- Customizable serializer options using `System.Text.Json`
- Easy, minimal configuration via `EazyClientOptions`
- Customizable `HttpMessageHandler` / delegating handlers
- Customizable retry policies
- Works with Blazor WASM and Microsoft Identity Platform

The examples scenarios are now covered (and kept up to date) by tests in this repository, primarily under `EazyHttp.Generator.Tests` and `EazyHttp.IntegrationTests`.

Minimum configuration
=====================================
-------------------------------------

The simplest way to use EazyHttp is to configure it at startup and let it generate a default client.

```csharp
var builder = WebApplication.CreateBuilder(args);

builder
  .Services
  .ConfigureEazyHttpClients()
  .AddEazyHttpClients();

var app = builder.Build();
```
With this configuration:
- A single default client is generated.
- Default web `JsonSerializerOptions` are used for both requests and responses.
- No custom headers, encodings, retry policies, or handlers are configured.

Clients configuration
=====================================
-------------------------------------

The configuration API is driven by `EazyClientOptions` and the `HttpClientDefinition` type from `EazyHttp.Abstractions`.

Notes:
- `NamespacePrefix` controls the root namespace of generated interfaces and implementations.
- `Clients` is the single source of truth for logical client names and base addresses.
- `RequestSerializerOptions` and `ResponseSerializerOptions` are now separate, and you can also plug in `ResolveRequestSerializer` / `ResolveResponseSerializer` for runtime decisions.
- `Retries`, `Encodings`, `PersistentHeaders`, and `HttpClientHandlers` are all keyed by the logical client name.

Chain of dependencies
=====================================
-------------------------------------

If multiple projects reference EazyHttp and you want to avoid analyzer / generator duplication, use `PrivateAssets="All"` on the package reference in downstream projects:

```xml
<ItemGroup>
  <PackageReference Include="EazyHttp" Version="x.y.z" PrivateAssets="All" />
</ItemGroup>
```

When more than one project configures EazyHttp clients, it is also advisable to use a custom namespace prefix to avoid generated type name collisions:

```csharp
services
  .ConfigureEazyHttpClients(opts =>
  {
    opts.NamespacePrefix = "CustomNameSpace";
  })
  .AddCustomNameSpaceClients();
```

Retries policy
=====================================
-------------------------------------

A retry policy can be specified per client via the `Retries` dictionary or obtained dynamically using `ResolveRetry`.

- If no retry policy is specified, the default assumes a single attempt (no retries).
- `StatusCodeMatchingCondition` lets you decide which responses should be retried based on status code and HTTP method.

You can also use `ResolveRetry` for runtime control

Custom HttpClientHandler
=====================================
-------------------------------------

A custom `HttpMessageHandler` (or `DelegatingHandler`) can be specified for each client through `HttpClientHandlers`.
The generator injects the configured handler type into the generated client registration pipeline.

- If the type derives from `DelegatingHandler`, it is added with `AddHttpMessageHandler<THandler>()`.
- Otherwise it is used as the primary handler via `ConfigurePrimaryHttpMessageHandler<THandler>()`.

Tests as examples
=====================================
-------------------------------------

Instead of maintaining a separate examples solution, this repository now relies on tests to demonstrate usage and validate behavior. For up-to-date examples, see:
- `EazyHttp.Generator.Tests` – covers generator configuration and generated client shapes.
- `EazyHttp.IntegrationTests` – covers end-to-end scenarios with real HTTP pipelines.

These tests serve as executable documentation and are updated alongside the library to avoid drift between documentation and implementation.



