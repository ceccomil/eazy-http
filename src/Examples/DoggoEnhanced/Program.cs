﻿global using Doggo;
global using DoggoEnhanced;
global using Microsoft.Extensions.DependencyInjection;
global using System.Diagnostics;
global using EazyHttp.Contracts;
global using EazyHttp;
global using DoggoEnhanced.EazyHttp;
global using System.Text.Json.Serialization;
global using DoggoEnhanced.Models;
global using System.Text.Json;
global using System.Text;
global using DoggoEnhanced.Helpers;

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

Console
    .WriteLine(
        $"Here is your picture: {newImageFileName}");

Process
    .Start(
        $"powershell.exe",
        newImageFileName);


Console
    .WriteLine(
        "Press Q to quit");

if (Console.ReadKey(true).Key != ConsoleKey.Q)
{
    goto Start;
}
