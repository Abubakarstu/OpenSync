using OpenSync.Core.Entities;

namespace OpenSync.Core.Interfaces.Repositories;

public interface IMapRepository : IRepository<SyncMap>
{
    Task<SyncMap?> GetByServiceAndNameAsync(Guid serviceId, string uniqueName, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SyncMap>> GetByServiceIdAsync(Guid serviceId, int skip = 0, int take = 50, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SyncMap>> GetExpiredAsync(CancellationToken cancellationToken = default);
}
