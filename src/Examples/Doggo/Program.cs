using Microsoft.Extensions.DependencyInjection;
using EazyHttp;
using System;
using Doggo;

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

await dogService
    .GetPicture();
