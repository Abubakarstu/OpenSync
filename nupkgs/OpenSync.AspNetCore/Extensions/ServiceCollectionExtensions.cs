using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenSync.Application;
using OpenSync.Infrastructure;
using OpenSync.Infrastructure.Configuration;

namespace OpenSync.AspNetCore.Extensions;

public class OpenSyncBuilder
{
    public IServiceCollection Services { get; }
    public OpenSyncBuilder(IServiceCollection services) => Services = services;
}

public static class ServiceCollectionExtensions
{
    public static OpenSyncBuilder AddOpenSync(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOpenSyncApplication();
        services.AddOpenSyncInfrastructure(configuration);
        return new OpenSyncBuilder(services);
    }

    public static OpenSyncBuilder AddOpenSync(this IServiceCollection services, Action<OpenSyncOptions> configureOptions)
    {
        var options = new OpenSyncOptions();
        configureOptions(options);
        services.Configure<OpenSyncOptions>(o =>
        {
            o.Limits = options.Limits;
            o.Heartbeat = options.Heartbeat;
            o.Ttl = options.Ttl;
            o.Backplane = options.Backplane;
            o.Database = options.Database;
        });

        services.AddOpenSyncApplication();
        services.AddOpenSyncInfrastructure(new ConfigurationBuilder().Build());
        return new OpenSyncBuilder(services);
    }
}
