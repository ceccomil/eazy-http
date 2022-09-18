global using Doggo;
global using DoggoEnhanced;
global using Microsoft.Extensions.DependencyInjection;
global using System.Diagnostics;
global using EazyHttp.Contracts;
global using EazyHttp;
global using DoggoEnhanced.EazyHttp;
global using System.Text.Json.Serialization;
global using DoggoEnhanced.Models;

//This example required a valid Computer Vision endpoint
//https://learn.microsoft.com/en-gb/azure/cognitive-services/computer-vision/
// A free version can be created from the azure portal

var services = new ServiceCollection()
    .ConfigureEazyHttpClients(opts =>
    {
        opts
            .NameSpacePrefix = "DoggoEnhanced";

        opts
            .EazyHttpClients
            .Add(new(
                "ComputerVision",
                "https://<computer vision service name>.cognitiveservices.azure.com/vision/v3.2/"));

        opts
            .PersistentHeaders
            .Add(
                "ComputerVision",
                new RequestHeader[]
                {
                    new(
                        "Ocp-Apim-Subscription-Key",
                        "<computer vision subscription key>")
                });

    })
    .AddEazyHttpClients()
    .AddTransient<IRandomDog, RandomDog>()
    .DoggoEnhancedAddEazyHttpClients()
    .AddTransient<IImageAnalysis, ImageAnalysis>();

using var sp = services
    .BuildServiceProvider();

using var scope = sp
    .CreateScope();

var dogService = scope
    .ServiceProvider
    .GetRequiredService<IRandomDog>();

var (data, fileName) = await dogService
    .GetAndSavePicture();

Console
    .WriteLine(
        $"Here is your picture: {fileName}");

Process
    .Start(
        $"powershell.exe",
        fileName);

var analysisService = scope
    .ServiceProvider
    .GetRequiredService<IImageAnalysis>();

var result = await analysisService
    .GetAnalysis(data);

Console
    .WriteLine(
        $"Analysis {result.Id} completed!");

foreach (var caption in result.Description.Captions)
{
    Console
        .WriteLine(
            caption);
}

Console
    .WriteLine(
        string
        .Join(
            ", ",
            result
            .Description
            .Tags
            .Select(x => $"#{x}")));

