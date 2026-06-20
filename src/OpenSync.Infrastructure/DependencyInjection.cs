using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenSync.Infrastructure.Configuration;
using OpenSync.Core.Interfaces.Clocks;
using OpenSync.Core.Interfaces.Repositories;
using OpenSync.Core.Interfaces.Services;
using OpenSync.Infrastructure.Auth;
using OpenSync.Infrastructure.Persistence;
using OpenSync.Infrastructure.Persistence.Interceptors;
using OpenSync.Infrastructure.Persistence.Repositories;
using OpenSync.Infrastructure.Realtime;
using OpenSync.Infrastructure.Realtime.Backplane;
using OpenSync.Infrastructure.Realtime.Connection;
using OpenSync.Infrastructure.Realtime.Dispatcher;
using OpenSync.Infrastructure.Realtime.Protocol;
using OpenSync.Infrastructure.Realtime.Subscriptions;
using OpenSync.Infrastructure.Realtime.Transports;
using OpenSync.Infrastructure.Services;

namespace OpenSync.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddOpenSyncInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var opensyncSection = configuration.GetSection("OpenSync");
        services.Configure<OpenSyncOptions>(opensyncSection);

        var dbOptions = opensyncSection.GetSection("Database");
        var provider = dbOptions["Provider"] ?? "PostgreSQL";
        var connectionString = dbOptions["ConnectionString"] ?? "Host=localhost;Database=opensync;Username=postgres;Password=postgres";

        services.AddDbContext<OpenSyncDbContext>((sp, options) =>
        {
            switch (provider)
            {
                case "Sqlite":
                    options.UseSqlite(connectionString);
                    break;
                case "SqlServer":
                    options.UseSqlServer(connectionString);
                    break;
                default:
                    options.UseNpgsql(connectionString);
                    break;
            }
        });

        services.AddScoped<AuditableEntityInterceptor>();
        services.AddScoped<DomainEventInterceptor>();

        services.AddScoped<ISyncServiceRepository, SyncServiceRepository>();
        services.AddScoped<IDocumentRepository, DocumentRepository>();
        services.AddScoped<IListRepository, ListRepository>();
        services.AddScoped<IListItemRepository, ListItemRepository>();
        services.AddScoped<IMapRepository, MapRepository>();
        services.AddScoped<IMapItemRepository, MapItemRepository>();
        services.AddScoped<IStreamRepository, StreamRepository>();
        services.AddScoped<IChannelRepository, ChannelRepository>();
        services.AddScoped<IChannelMemberRepository, ChannelMemberRepository>();

        services.AddSingleton<IConnectionManager, ConnectionManager>();
        services.AddSingleton<ISubscriptionManager, SubscriptionManager>();
        services.AddSingleton<IMessageSerializer, MessageSerializer>();
        services.AddSingleton<ITransport, WebSocketTransport>();
        services.AddSingleton<IEventDispatcher, EventDispatcher>();
        services.AddSingleton<FanOutStrategy>();
        services.AddSingleton<IRealtimeNotifier, RealtimeNotifier>();

        var backplaneType = opensyncSection.GetSection("Backplane")["Type"] ?? "InProcess";
        if (backplaneType == "Redis")
            services.AddSingleton<IBackplane, RedisBackplane>();
        else
            services.AddSingleton<IBackplane, InProcessBackplane>();

        services.AddSingleton<ITokenService, JwtTokenService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddSingleton<IIdGenerator, IdGenerator>();
        services.AddSingleton<ISystemClock, SystemClock>();
        services.AddSingleton<IJsonValidator, JsonValidator>();
        services.AddSingleton<RateLimitService>();
        services.AddHostedService<TtlExpiryService>();
        services.AddHostedService<HeartbeatService>();
        services.AddHostedService<ConnectionCleanupService>();

        return services;
    }
}
