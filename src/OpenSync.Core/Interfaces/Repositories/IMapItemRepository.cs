using OpenSync.Core.Entities;

namespace OpenSync.Core.Interfaces.Repositories;

public interface IMapItemRepository : IRepository<SyncMapItem>
{
    Task<SyncMapItem?> GetByMapIdAndKeyAsync(Guid mapId, string key, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SyncMapItem>> GetByMapIdAsync(Guid mapId, int skip = 0, int take = 50, CancellationToken cancellationToken = default);
    Task<bool> KeyExistsAsync(Guid mapId, string key, CancellationToken cancellationToken = default);
    Task<int> GetCountByMapIdAsync(Guid mapId, CancellationToken cancellationToken = default);
    Task DeleteByMapIdAsync(Guid mapId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SyncMapItem>> GetExpiredAsync(CancellationToken cancellationToken = default);
}
