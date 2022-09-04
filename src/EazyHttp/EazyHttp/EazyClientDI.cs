namespace EazyHttp;

/// <summary>
/// DI registration
/// </summary>
public static class EazyClientDI
{
    /// <summary>
    /// TODO documentation
    /// </summary>
    /// <param name="services"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static IServiceCollection ConfigureEazyHttpClients(
        this IServiceCollection services,
        Action<EazyClientOptions>? config = default)
    {
        config ??= delegate { };

        return services
            .Configure(config);
    }
}
