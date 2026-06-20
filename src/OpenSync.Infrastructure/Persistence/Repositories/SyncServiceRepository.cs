using Microsoft.EntityFrameworkCore;
using OpenSync.Core.Entities;
using OpenSync.Core.Interfaces.Repositories;

namespace OpenSync.Infrastructure.Persistence.Repositories;

public class SyncServiceRepository : BaseRepository<SyncService>, ISyncServiceRepository
{
    public SyncServiceRepository(OpenSyncDbContext context) : base(context) { }

    public async Task<SyncService?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        => await DbSet.FirstOrDefaultAsync(s => s.Name == name, cancellationToken);

    public async Task<bool> NameExistsAsync(string name, CancellationToken cancellationToken = default)
        => await DbSet.AnyAsync(s => s.Name == name, cancellationToken);
}
