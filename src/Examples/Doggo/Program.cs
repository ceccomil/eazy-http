global using Doggo;
using EazyHttp;
using Doggo.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;

var services = new ServiceCollection()
    .ConfigureEazyHttpClients(x =>
    {
        x.NamespacePrefix = "Doggo.Http";

        x.Clients.Add(new("DoggoClient", IRandomDog.BaseUrl, false));

        x
        .SerializerOptions.Add(
            "DoggoClient",
            IRandomDog.JsonOptions);
    })
    .AddEazyHttpClients()
    .AddTransient<IRandomDog, RandomDog>();

using var sp = services
    .BuildServiceProvider();

using var scope = sp
    .CreateScope();

var dogService = scope
    .ServiceProvider
    .GetRequiredService<IRandomDog>();

var (_, fileName) = await dogService
    .GetAndSavePicture();

Console
    .WriteLine(
    $"Here is your picture: {fileName}");

Process.Start($"powershell.exe", fileName);
