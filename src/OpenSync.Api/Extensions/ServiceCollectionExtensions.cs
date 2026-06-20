namespace OpenSync.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOpenSyncApiServices(this IServiceCollection services)
    {
        services.AddScoped<OpenSync.Api.WebSockets.SyncWebSocketEndpoint>();
        return services;
    }
}
