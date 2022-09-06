using Microsoft.Extensions.DependencyInjection;
using EazyHttp;
using EazyHttp.HttpClients;

var services = new ServiceCollection()
    .ConfigureEazyHttpClients()
    .AddEazyHttpClients();

Console.WriteLine("Hello, World!");
