global using Doggo;
global using Microsoft.Extensions.DependencyInjection;
global using System.Diagnostics;
global using EazyHttp;

var services = new ServiceCollection()
    .ConfigureEazyHttpClients(opts =>
    {
        opts
            .NameSpacePrefix = "DoggoEnhanced";

        opts
            .EazyHttpClients
            .Add(new("DoggoClient", ""));

        opts
            .EazyHttpClients
            .Add(new("MsClient", ""));

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

var fileName = await dogService
    .SavePicture();

Console
    .WriteLine(
    $"Here is your picture: {fileName}");

Process.Start($"powershell.exe", fileName);
