global using Doggo;
global using DoggoEnhanced;
global using DoggoEnhanced.EazyHttp;
global using DoggoEnhanced.Helpers;
global using DoggoEnhanced.Models;
global using EazyHttp;
global using EazyHttp.Contracts;
global using Microsoft.Extensions.DependencyInjection;
global using System.Diagnostics;
global using System.Text;
global using System.Text.Json;
global using System.Text.Json.Serialization;

// This example required a valid Computer Vision endpoint
// https://learn.microsoft.com/en-gb/azure/cognitive-services/computer-vision/
// A free version can be created from the azure portal

var services = new ServiceCollection()
    .RegisterServices();

using var sp = services
    .BuildServiceProvider();

using var scope = sp
    .CreateScope();

var dogService = scope
    .ServiceProvider
    .GetRequiredService<IRandomDog>();

Start:
var (data, fileName) = await dogService
    .GetAndSavePicture();

var source = new FileInfo(fileName);

Console.WriteLine(
    $"Here is your picture: {source.FullName}");

Process.Start(
    "explorer.exe",
    "\"" + source.FullName + "\"");

var analysisService = scope
    .ServiceProvider
    .GetRequiredService<IImageAnalysis>();

var result = await analysisService
    .GetAnalysis(data);

Console
    .WriteLine(
        $"Analysis {result.Id} completed:");

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


var text = result
    .Description
    .Captions
    .OrderByDescending(x => x.Confidence)
    .FirstOrDefault()
    ?.Text;

if (text is null)
{
    return;
}

Console
    .WriteLine(
        "Please wait, we are getting a new AI generated image...");

var (_, newImageFileName) = await analysisService
    .GetAndSaveAiPicture(
        text);

var aiImg = new FileInfo(newImageFileName);

Console.WriteLine(
    $"Here is your picture: {aiImg.FullName}");

Process.Start(
    "explorer.exe",
    "\"" + aiImg.FullName + "\"");


Console.WriteLine("Press Q to quit");

if (Console.ReadKey(true).Key != ConsoleKey.Q)
{
    goto Start;
}
