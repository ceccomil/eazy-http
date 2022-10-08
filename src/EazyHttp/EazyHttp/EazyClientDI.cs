namespace EazyHttp;

/// 
public static class EazyClientDI
{
    /// <summary>
    /// Add the EazyHttpClients configuration
    /// </summary>
    public static IServiceCollection ConfigureEazyHttpClients(
        this IServiceCollection services,
        Action<EazyClientOptions>? config = default)
    {
        config ??= delegate { };

        return services
            .Configure(config);
    }
}
