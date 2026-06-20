using OpenSync.Core.Entities;

namespace OpenSync.Core.Interfaces.Repositories;

public interface IChannelRepository : IRepository<SyncChannel>
{
    Task<SyncChannel?> GetByServiceAndNameAsync(Guid serviceId, string uniqueName, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SyncChannel>> GetByServiceIdAsync(Guid serviceId, int skip = 0, int take = 50, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SyncChannel>> GetExpiredAsync(CancellationToken cancellationToken = default);
}
