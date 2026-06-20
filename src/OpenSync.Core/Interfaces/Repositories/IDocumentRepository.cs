using OpenSync.Core.Entities;

namespace OpenSync.Core.Interfaces.Repositories;

public interface IDocumentRepository : IRepository<SyncDocument>
{
    Task<SyncDocument?> GetByServiceAndNameAsync(Guid serviceId, string uniqueName, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SyncDocument>> GetByServiceIdAsync(Guid serviceId, int skip = 0, int take = 50, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SyncDocument>> GetExpiredAsync(CancellationToken cancellationToken = default);
}
