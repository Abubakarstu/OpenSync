using Microsoft.EntityFrameworkCore;
using OpenSync.Core.Entities;
using OpenSync.Core.Interfaces.Repositories;

namespace OpenSync.Infrastructure.Persistence.Repositories;

public class ChannelRepository : BaseRepository<SyncChannel>, IChannelRepository
{
    public ChannelRepository(OpenSyncDbContext context) : base(context) { }

    public async Task<SyncChannel?> GetByServiceAndNameAsync(Guid serviceId, string uniqueName, CancellationToken cancellationToken = default)
        => await DbSet.FirstOrDefaultAsync(c => c.ServiceId == serviceId && c.UniqueName == uniqueName, cancellationToken);

    public async Task<IReadOnlyList<SyncChannel>> GetByServiceIdAsync(Guid serviceId, int skip = 0, int take = 50, CancellationToken cancellationToken = default)
        => await DbSet.Where(c => c.ServiceId == serviceId).OrderByDescending(c => c.UpdatedAt).Skip(skip).Take(take).ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<SyncChannel>> GetExpiredAsync(CancellationToken cancellationToken = default)
        => await DbSet.Where(c => c.ExpiresAt != null && c.ExpiresAt < DateTime.UtcNow).ToListAsync(cancellationToken);
}
