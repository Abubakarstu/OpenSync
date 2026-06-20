using OpenSync.Core.Entities;

namespace OpenSync.Core.Interfaces.Repositories;

public interface IListRepository : IRepository<SyncList>
{
    Task<SyncList?> GetByServiceAndNameAsync(Guid serviceId, string uniqueName, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SyncList>> GetByServiceIdAsync(Guid serviceId, int skip = 0, int take = 50, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SyncList>> GetExpiredAsync(CancellationToken cancellationToken = default);
}
