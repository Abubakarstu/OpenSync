using Microsoft.Extensions.DependencyInjection;
using OpenSync.Sdk.Auth;
using OpenSync.Sdk.Transports;

namespace OpenSync.Sdk.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOpenSyncClient(this IServiceCollection services, string baseUrl, TransportType transportType = TransportType.WebSocket)
    {
        services.AddSingleton<ITokenProvider>(sp => new TokenProvider("default-api-key"));
        services.AddSingleton(sp => new OpenSyncClient(baseUrl, sp.GetRequiredService<ITokenProvider>(), transportType));
        return services;
    }
}
