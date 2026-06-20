using Microsoft.EntityFrameworkCore;
using OpenSync.Core.Entities;
using OpenSync.Core.Interfaces.Repositories;

namespace OpenSync.Infrastructure.Persistence.Repositories;

public class MapRepository : BaseRepository<SyncMap>, IMapRepository
{
    public MapRepository(OpenSyncDbContext context) : base(context) { }

    public async Task<SyncMap?> GetByServiceAndNameAsync(Guid serviceId, string uniqueName, CancellationToken cancellationToken = default)
        => await DbSet.FirstOrDefaultAsync(m => m.ServiceId == serviceId && m.UniqueName == uniqueName, cancellationToken);

    public async Task<IReadOnlyList<SyncMap>> GetByServiceIdAsync(Guid serviceId, int skip = 0, int take = 50, CancellationToken cancellationToken = default)
        => await DbSet.Where(m => m.ServiceId == serviceId).OrderByDescending(m => m.UpdatedAt).Skip(skip).Take(take).ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<SyncMap>> GetExpiredAsync(CancellationToken cancellationToken = default)
        => await DbSet.Where(m => m.ExpiresAt != null && m.ExpiresAt < DateTime.UtcNow).ToListAsync(cancellationToken);
}
