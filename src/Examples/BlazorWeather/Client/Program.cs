using EazyHttp;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace BlazorWeather.Client;

public static class Program
{
    public static string BaseAddress { get; private set; } = null!;

    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        BaseAddress = builder.HostEnvironment.BaseAddress;

        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder
            .Services
            .ConfigureEazyHttpClients(opts =>
            {
                opts
                    .EazyHttpClients.Add(new(
                        "BlazorClient",
                        BlazorWeather.Client.Program.BaseAddress));

                opts
                    .HttpClientHandlers.Add(
                        "BlazorClient",
                        "BlazorWeather.Client.AuthMessageHandler");
            })
            .AddEazyHttpClients()
            .AddTransient<AuthMessageHandler.AuthClientHandler>()
            .AddMsalAuthentication(options =>
            {
                builder
                    .Configuration
                    .Bind(
                        "AzureAd",
                        options.ProviderOptions.Authentication);

                options
                    .ProviderOptions
                    .DefaultAccessTokenScopes
                    .Add(
                        builder.Configuration.GetSection("ServerApi")["Scopes"]!);
            });

        await builder
            .Build()
            .RunAsync();
    }
}