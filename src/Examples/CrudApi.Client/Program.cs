using Microsoft.Extensions.DependencyInjection;
using EazyHttp;
using EazyHttp.Contracts;
using CrudApi.Client;
using System.Text.Json;
using System.Text.Json.Serialization;

Console
    .WriteLine(
        "Press a key when the WEB API " +
        "project has started to beging fetching data");

_ = Console
    .ReadKey(intercept: true);

var services = new ServiceCollection()
    .ConfigureEazyHttpClients(
        opts =>
        {
            opts
                .EazyHttpClients
                .Add(new(
                    "CrudApiClient",
                    "https://localhost:7240/"));

            opts
                .PersistentHeaders
                .Add(
                    "CrudApiClient",
                    new RequestHeader[]
                    {
                        new(
                            "X-SharedSecret",
                            "RSD1slpdGwRKrhq4QtWbPRalgahrRkjY6AaxVusbI1qlM7mIc3dUWqcbhFAc3cPKyZHnLMd2e3sjXYkfxzMBCDZX3t0DiMV9NUZU0UbdITkWAXtpvfcpvJASuRZln3So")
                    });

            opts
                .SerializersOptions
                .Add(
                    "CrudApiClient",
                    new(
                        JsonSerializerDefaults.Web)
                        {
                            WriteIndented = true,
                            DefaultIgnoreCondition = JsonIgnoreCondition
                                    .WhenWritingNull
                        });
        })
    .AddEazyHttpClients();

using var sp = services
    .BuildServiceProvider();

using var scope = sp
    .CreateScope();

var clients = scope
    .ServiceProvider
    .GetRequiredService<IEazyClients>();

var orders = await clients
    .CrudApiClient
    .GetAsync<IEnumerable<Order>>("Orders")
    ?? throw new NullReferenceException();

foreach (var o in orders)
{
    Console
        .WriteLine(o);
    
    Console
        .WriteLine();
}

Console
    .WriteLine(
        "Press A to add a new order, any other key to skip.");

var key = Console
    .ReadKey(intercept: true);

if (key.Key == ConsoleKey.A)
{
    var order = new Order();

    Console
        .WriteLine(
            "Please, type the name of the customer: ");

    order.CustomerName = Console
        .ReadLine();

    Console
        .WriteLine(
            "Please, type the description of the order: ");

    order.Description = Console
        .ReadLine();

    Console
        .WriteLine(
            "Please, type the amount of the order: ");

    order.Amount = Convert
        .ToDecimal(
            Console
            .ReadLine());

    Console
        .WriteLine(
            $"Order accepted{Environment.NewLine}" +
            await clients
            .CrudApiClient
            .PostAsync<Order>(
                "Orders",
                order));

    Console
        .WriteLine();
}

Console
    .WriteLine(
        "Press a key to quit!");

_ = Console
    .ReadKey(intercept: true);
