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
- Without a custom configuration a single client will be created, with web defaults serializer and no custom headers attached to requests

Clients configuration
=====================================
-------------------------------------

```csharp
...
services
    .ConfigureEazyHttpClients(
        opts =>
        {
            opts
                .EazyHttpClients
                .Add(new(
                    "MyClientName",
                    "https://baseAddress/"));

            opts
                .PersistentHeaders
                .Add(
                    "MyClientName",
                    new RequestHeader[]
                    {
                        new(
                            "HeaderKey",
                            "HeaderValue"),
                        new(
                            "SecondHeaderKey",
                            "SecondHeaderValue")
                    });

            opts
                .SerializersOptions
                .Add(
                    "MyClientName",
                    new JsonSerializerOptions());
        })
    .AddEazyHttpClients();
```
See example: CrudApi + CrudApi.Client
- With a custom configuration clients can be added, and for each it's possible to specify a custom JsonSerializerOptions as well as a collection of request headers

Chain of dependencies
=====================================
-------------------------------------

If multiple projects are chained and reference to EazyHttp is specified in more than one, to avoid duplications of the analyzer it's advisable to add a PrivateAssets="All" attribute
i.e.
```
  <ItemGroup>
    <PackageReference Include="EazyHttp" Version="0.2.0-beta" PrivateAssets="All" />
  </ItemGroup>
```

In addition to this, it's also advisable to use a custom namespace prefix in the configuration, if more than one project is setting up EazyHttpClients

```csharp
...
services
    .ConfigureEazyHttpClients(opts =>
    {
        opts
            .NameSpacePrefix = "CustomNameSpace";
    })
    .CustomNameSpaceAddEazyHttpClients()
```
See example: DoggoEnhanced
- The project is referencing the Doggo example and using the RandomDog service in addition to its own service to consume Microsoft Computer Vision.
In order to run the example a free Computer Vision service must be created and in Program.cs the URL and the subscription key must be changed.
see:
https://learn.microsoft.com/en-gb/azure/cognitive-services/computer-vision/
https://learn.microsoft.com/en-gb/azure/cognitive-services/computer-vision/quickstarts-sdk/image-analysis-client-library?tabs=visual-studio&pivots=programming-language-csharp#prerequisites


