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
- The project is referencing the Doggo example and using the RandomDog service in addition to its own service to consume [Microsoft Computer Vision](https://learn.microsoft.com/en-gb/azure/cognitive-services/computer-vision/) and [DeepAi](https://deepai.org/machine-learning-model/text2img).
- In order to run the example a free Computer Vision service must be created and in Program.cs the URL and the subscription key must be changed, the quickstart api-key is provided for consuming DeepAi.
[Computer Vision Prerequisites](https://learn.microsoft.com/en-gb/azure/cognitive-services/computer-vision/quickstarts-sdk/image-analysis-client-library?tabs=visual-studio&pivots=programming-language-csharp#prerequisites)
- Please note that a custom serializer is in use for this example

Retries policy
=====================================
-------------------------------------

A retry policy can be specified for each client in a similar manner as setting the serializer or required headers.
Each retry will have an exponential backoff as well as a random component, a seed can be set to keep results consistent

```csharp
...
services
    .ConfigureEazyHttpClients(
        opts =>
        {
            opts
                .EazyHttpClients
                .Add(new(
                    "MyClient1",
                    "https://localhost:1/"));

            opts
                .EazyHttpClients
                .Add(new(
                    "MyClient2",
                    "https://localhost:2/"));

            opts
                .Retries
                .Add(
                    "MyClient1",
                    new()
                    {
                        MaxAttempts = 5,
                        Seed = 12345
                    });

            opts
                .Retries
                .Add(
                    "MyClient2",
                    new()
                    {
                        MaxAttempts = 5,
                        StatusCodeMatchingCondition = (status, method) =>
                        {
                            if (status is HttpStatusCode.ServiceUnavailable
                                or HttpStatusCode.GatewayTimeout
                                or HttpStatusCode.RequestTimeout)
                            {
                                return true;
                            }

                            if (status is HttpStatusCode.TooManyRequests &&
                                method == HttpMethod.Get)
                            {
                                return true;
                            }

                            return false;
                        }
                    });
        })
        ...
```
See example: CrudApi.Client.Retries
- If no retry policy is specified the default assumes 1 attempt therfore no retries on failure
- In the example above, both clients are setting up a policy with 5 maximum retries
- The second client has a configuration to establish which cases will be retried
- The default implementation of `StatusCodeMatchingCondition`, first client, assumes retries only in case of 429 or 503 regardless of the http method.

Custom HttpClientHandler
=====================================
-------------------------------------

A custom HttpClientHandler can be specified for each client in a similar manner as setting the serializer or required headers.
The generator will add the name provide as one of the injected parameters of the EazyHttpClient class.
Please make sure that the name provided is a fully qualified name (or use global using) and the handler as been registered to the DI pipeline.

```csharp
...
services
    .ConfigureEazyHttpClients(
        opts =>
        {
            opts
                .EazyHttpClients
                .Add(new(
                    "MyClient1",
                    "https://localhost:1/"));

            opts
                .HttpClientHandlers
                .Add(
                    "MessariClient",
                    "MyNameSpace.CustomHttpHandler");
        })
        .AddTransient<CustomHttpHandler>();
        ...
```
See example: CryptoPrices



