# EazyHttp
Friendly and helpful library to handle HTTP requests in .NET projects
=====================================================================

------------------------------------------------------------------
Source: [GitHub repo](https://github.com/ceccomil/eazy-http)

Packages
--------
| Package | NuGet Stable | NuGet Pre-release | Downloads |
| ------- | ------------ | ----------------- | --------- | 
| [EazyHttp](https://www.nuget.org/packages/EazyHttp) | [![CaptainLogger](https://img.shields.io/nuget/v/EazyHttp.svg)](https://www.nuget.org/packages/EazyHttp) | [![CaptainLogger](https://img.shields.io/nuget/vpre/EazyHttp.svg)](https://www.nuget.org/packages/EazyHttp/) | [![CaptainLogger](https://img.shields.io/nuget/dt/EazyHttp.svg)](https://www.nuget.org/packages/EazyHttp/) |

Features
--------
- Typed Http client wrapper to perform requests
- Customizable serializer options using System.Text.Json
- Easy, minimal configuration

Minimum configuration
=====================================
-------------------------------------

```csharp
...
var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .ConfigureEazyHttpClients()
    .AddEazyHttpClients();

var app = builder.Build();
```
See example: Doggo


