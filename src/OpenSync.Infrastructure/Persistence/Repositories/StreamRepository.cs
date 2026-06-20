using Microsoft.EntityFrameworkCore;
using OpenSync.Core.Entities;
using OpenSync.Core.Interfaces.Repositories;

namespace OpenSync.Infrastructure.Persistence.Repositories;

public class StreamRepository : BaseRepository<SyncStream>, IStreamRepository
{
    public StreamRepository(OpenSyncDbContext context) : base(context) { }

    public async Task<SyncStream?> GetByServiceAndNameAsync(Guid serviceId, string uniqueName, CancellationToken cancellationToken = default)
        => await DbSet.FirstOrDefaultAsync(s => s.ServiceId == serviceId && s.UniqueName == uniqueName, cancellationToken);

    public async Task<IReadOnlyList<SyncStream>> GetByServiceIdAsync(Guid serviceId, int skip = 0, int take = 50, CancellationToken cancellationToken = default)
        => await DbSet.Where(s => s.ServiceId == serviceId).OrderByDescending(s => s.UpdatedAt).Skip(skip).Take(take).ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<SyncStream>> GetExpiredAsync(CancellationToken cancellationToken = default)
        => await DbSet.Where(s => s.ExpiresAt != null && s.ExpiresAt < DateTime.UtcNow).ToListAsync(cancellationToken);
}
