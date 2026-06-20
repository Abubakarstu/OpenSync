using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenSync.Core.Interfaces.Repositories;
using OpenSync.Infrastructure.Configuration;
using OpenSync.Infrastructure.Realtime.Protocol;

namespace OpenSync.Infrastructure.Services;

public class TtlExpiryService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<TtlExpiryService> _logger;
    private readonly int _cleanupIntervalSeconds;

    public TtlExpiryService(IServiceScopeFactory scopeFactory, ILogger<TtlExpiryService> logger, IOptions<OpenSyncOptions> options)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _cleanupIntervalSeconds = options.Value.Ttl.CleanupIntervalSeconds;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("TTL expiry service started with interval {Interval}s", _cleanupIntervalSeconds);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromSeconds(_cleanupIntervalSeconds), stoppingToken);

            try
            {
                using var scope = _scopeFactory.CreateScope();
                await CleanupDocumentsAsync(scope, stoppingToken);
                await CleanupListsAsync(scope, stoppingToken);
                await CleanupMapsAsync(scope, stoppingToken);
                await CleanupStreamsAsync(scope, stoppingToken);
                await CleanupChannelsAsync(scope, stoppingToken);
            }
            catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogError(ex, "TTL cleanup failed");
            }
        }
    }

    private async Task CleanupDocumentsAsync(IServiceScope scope, CancellationToken ct)
    {
        var repo = scope.ServiceProvider.GetRequiredService<IDocumentRepository>();
        var expired = await repo.GetExpiredAsync(ct);
        if (expired.Count > 0)
        {
            await repo.DeleteRangeAsync(expired, ct);
            await repo.SaveChangesAsync(ct);
            _logger.LogInformation("Cleaned up {Count} expired documents", expired.Count);
        }
    }

    private async Task CleanupListsAsync(IServiceScope scope, CancellationToken ct)
    {
        var repo = scope.ServiceProvider.GetRequiredService<IListRepository>();
        var itemRepo = scope.ServiceProvider.GetRequiredService<IListItemRepository>();
        var expired = await repo.GetExpiredAsync(ct);
        foreach (var list in expired)
        {
            await itemRepo.DeleteByListIdAsync(list.Id, ct);
            await repo.DeleteAsync(list, ct);
        }
        if (expired.Count > 0)
        {
            await repo.SaveChangesAsync(ct);
            _logger.LogInformation("Cleaned up {Count} expired lists", expired.Count);
        }
    }

    private async Task CleanupMapsAsync(IServiceScope scope, CancellationToken ct)
    {
        var repo = scope.ServiceProvider.GetRequiredService<IMapRepository>();
        var itemRepo = scope.ServiceProvider.GetRequiredService<IMapItemRepository>();
        var expired = await repo.GetExpiredAsync(ct);
        foreach (var map in expired)
        {
            await itemRepo.DeleteByMapIdAsync(map.Id, ct);
            await repo.DeleteAsync(map, ct);
        }
        if (expired.Count > 0)
        {
            await repo.SaveChangesAsync(ct);
            _logger.LogInformation("Cleaned up {Count} expired maps", expired.Count);
        }
    }

    private async Task CleanupStreamsAsync(IServiceScope scope, CancellationToken ct)
    {
        var repo = scope.ServiceProvider.GetRequiredService<IStreamRepository>();
        var expired = await repo.GetExpiredAsync(ct);
        if (expired.Count > 0)
        {
            await repo.DeleteRangeAsync(expired, ct);
            await repo.SaveChangesAsync(ct);
            _logger.LogInformation("Cleaned up {Count} expired streams", expired.Count);
        }
    }

    private async Task CleanupChannelsAsync(IServiceScope scope, CancellationToken ct)
    {
        var repo = scope.ServiceProvider.GetRequiredService<IChannelRepository>();
        var memberRepo = scope.ServiceProvider.GetRequiredService<IChannelMemberRepository>();
        var expired = await repo.GetExpiredAsync(ct);
        foreach (var channel in expired)
        {
            await memberRepo.DeleteByChannelIdAsync(channel.Id, ct);
            await repo.DeleteAsync(channel, ct);
        }
        if (expired.Count > 0)
        {
            await repo.SaveChangesAsync(ct);
            _logger.LogInformation("Cleaned up {Count} expired channels", expired.Count);
        }
    }
}
