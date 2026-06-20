using OpenSync.Core.Entities;

namespace OpenSync.Core.Interfaces.Repositories;

public interface ISyncServiceRepository : IRepository<SyncService>
{
    Task<SyncService?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<bool> NameExistsAsync(string name, CancellationToken cancellationToken = default);
}
