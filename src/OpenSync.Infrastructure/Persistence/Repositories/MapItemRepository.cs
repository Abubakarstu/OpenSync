using Microsoft.EntityFrameworkCore;
using OpenSync.Core.Entities;
using OpenSync.Core.Interfaces.Repositories;

namespace OpenSync.Infrastructure.Persistence.Repositories;

public class MapItemRepository : BaseRepository<SyncMapItem>, IMapItemRepository
{
    public MapItemRepository(OpenSyncDbContext context) : base(context) { }

    public async Task<SyncMapItem?> GetByMapIdAndKeyAsync(Guid mapId, string key, CancellationToken cancellationToken = default)
        => await DbSet.FirstOrDefaultAsync(i => i.MapId == mapId && i.Key == key, cancellationToken);

    public async Task<IReadOnlyList<SyncMapItem>> GetByMapIdAsync(Guid mapId, int skip = 0, int take = 50, CancellationToken cancellationToken = default)
        => await DbSet.Where(i => i.MapId == mapId).OrderBy(i => i.Key).Skip(skip).Take(take).ToListAsync(cancellationToken);

    public async Task<bool> KeyExistsAsync(Guid mapId, string key, CancellationToken cancellationToken = default)
        => await DbSet.AnyAsync(i => i.MapId == mapId && i.Key == key, cancellationToken);

    public async Task<int> GetCountByMapIdAsync(Guid mapId, CancellationToken cancellationToken = default)
        => await DbSet.CountAsync(i => i.MapId == mapId, cancellationToken);

    public async Task DeleteByMapIdAsync(Guid mapId, CancellationToken cancellationToken = default)
    {
        var items = await DbSet.Where(i => i.MapId == mapId).ToListAsync(cancellationToken);
        DbSet.RemoveRange(items);
    }

    public async Task<IReadOnlyList<SyncMapItem>> GetExpiredAsync(CancellationToken cancellationToken = default)
        => await DbSet.Where(i => i.ExpiresAt != null && i.ExpiresAt < DateTime.UtcNow).ToListAsync(cancellationToken);
}
