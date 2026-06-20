using Microsoft.EntityFrameworkCore;
using OpenSync.Core.Entities;
using OpenSync.Core.Interfaces.Repositories;

namespace OpenSync.Infrastructure.Persistence.Repositories;

public class DocumentRepository : BaseRepository<SyncDocument>, IDocumentRepository
{
    public DocumentRepository(OpenSyncDbContext context) : base(context) { }

    public async Task<SyncDocument?> GetByServiceAndNameAsync(Guid serviceId, string uniqueName, CancellationToken cancellationToken = default)
        => await DbSet.FirstOrDefaultAsync(d => d.ServiceId == serviceId && d.UniqueName == uniqueName, cancellationToken);

    public async Task<IReadOnlyList<SyncDocument>> GetByServiceIdAsync(Guid serviceId, int skip = 0, int take = 50, CancellationToken cancellationToken = default)
        => await DbSet.Where(d => d.ServiceId == serviceId).OrderByDescending(d => d.UpdatedAt).Skip(skip).Take(take).ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<SyncDocument>> GetExpiredAsync(CancellationToken cancellationToken = default)
        => await DbSet.Where(d => d.ExpiresAt != null && d.ExpiresAt < DateTime.UtcNow).ToListAsync(cancellationToken);
}
