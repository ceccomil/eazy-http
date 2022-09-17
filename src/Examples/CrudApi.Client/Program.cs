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
        "Press U to update an order or, any other key to skip.");

key = Console
    .ReadKey(intercept: true);

if (key.Key == ConsoleKey.U)
{
    var order = new Order();

    Console
        .WriteLine(
            "Please, type (or paste) the id of the order: ");

    var id = Console
        .ReadLine();

    Console
        .WriteLine(
            "Please, type the new name of the customer: ");

    order.CustomerName = Console
        .ReadLine();

    Console
        .WriteLine(
            "Please, type the new description of the order: ");

    order.Description = Console
        .ReadLine();

    Console
        .WriteLine(
            "Please, type the new amount of the order: ");

    order.Amount = Convert
        .ToDecimal(
            Console
            .ReadLine());

    Console
        .WriteLine(
            $"Order updated{Environment.NewLine}" +
            await clients
            .CrudApiClient
            .PutAsync<Order>(
                $"Orders/{id}",
                order));

    Console
        .WriteLine();
}

Console
    .WriteLine(
        "Press D to delete an order or, any other key to skip.");

key = Console
    .ReadKey(intercept: true);

if (key.Key == ConsoleKey.D)
{
    Console
        .WriteLine(
            "Please, type (or paste) the id of the order: ");

    var id = Console
        .ReadLine();

    Console
        .WriteLine(
            await clients
            .CrudApiClient
            .DeleteAsync<string>(
                $"Orders/{id}"));

    Console
        .WriteLine();
}

Console
    .WriteLine(
        "Press P to change an order amount or, any other key to skip.");

key = Console
    .ReadKey(intercept: true);

if (key.Key == ConsoleKey.P)
{
    Console
        .WriteLine(
            "Please, type (or paste) the id of the order: ");

    var id = Console
        .ReadLine();

    Console
        .WriteLine(
            "Please, type the new amount of the order: ");

    var amount = Convert
        .ToDecimal(
            Console
            .ReadLine());

    Console
        .WriteLine(
            $"Order amount changed {Environment.NewLine}" +
            await clients
            .CrudApiClient
            .PatchAsync<Order>(
                $"Orders/{id}",
                new Order()
                {
                    Amount = amount
                }));

    Console
        .WriteLine();
}

Console
    .WriteLine(
        "Press a key to quit!");

_ = Console
    .ReadKey(intercept: true);
