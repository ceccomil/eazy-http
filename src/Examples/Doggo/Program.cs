using Microsoft.Extensions.DependencyInjection;
using EazyHttp;
using Doggo;
using System.Diagnostics;
using System;

var services = new ServiceCollection()
    .ConfigureEazyHttpClients()
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
