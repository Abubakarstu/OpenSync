using OpenSync.Core.Entities;

namespace OpenSync.Core.Interfaces.Repositories;

public interface IStreamRepository : IRepository<SyncStream>
{
    Task<SyncStream?> GetByServiceAndNameAsync(Guid serviceId, string uniqueName, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SyncStream>> GetByServiceIdAsync(Guid serviceId, int skip = 0, int take = 50, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SyncStream>> GetExpiredAsync(CancellationToken cancellationToken = default);
}
