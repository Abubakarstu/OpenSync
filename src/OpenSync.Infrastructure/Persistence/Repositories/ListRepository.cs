using Microsoft.EntityFrameworkCore;
using OpenSync.Core.Entities;
using OpenSync.Core.Interfaces.Repositories;

namespace OpenSync.Infrastructure.Persistence.Repositories;

public class ListRepository : BaseRepository<SyncList>, IListRepository
{
    public ListRepository(OpenSyncDbContext context) : base(context) { }

    public async Task<SyncList?> GetByServiceAndNameAsync(Guid serviceId, string uniqueName, CancellationToken cancellationToken = default)
        => await DbSet.FirstOrDefaultAsync(l => l.ServiceId == serviceId && l.UniqueName == uniqueName, cancellationToken);

    public async Task<IReadOnlyList<SyncList>> GetByServiceIdAsync(Guid serviceId, int skip = 0, int take = 50, CancellationToken cancellationToken = default)
        => await DbSet.Where(l => l.ServiceId == serviceId).OrderByDescending(l => l.UpdatedAt).Skip(skip).Take(take).ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<SyncList>> GetExpiredAsync(CancellationToken cancellationToken = default)
        => await DbSet.Where(l => l.ExpiresAt != null && l.ExpiresAt < DateTime.UtcNow).ToListAsync(cancellationToken);
}
