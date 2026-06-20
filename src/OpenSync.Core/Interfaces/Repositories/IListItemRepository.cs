using OpenSync.Core.Entities;

namespace OpenSync.Core.Interfaces.Repositories;

public interface IListItemRepository : IRepository<SyncListItem>
{
    Task<IReadOnlyList<SyncListItem>> GetByListIdAsync(Guid listId, int skip = 0, int take = 50, CancellationToken cancellationToken = default);
    Task<int> GetCountByListIdAsync(Guid listId, CancellationToken cancellationToken = default);
    Task<SyncListItem?> GetByListIdAndIndexAsync(Guid listId, int index, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SyncListItem>> GetItemsFromIndexAsync(Guid listId, int fromIndex, CancellationToken cancellationToken = default);
    Task DeleteByListIdAsync(Guid listId, CancellationToken cancellationToken = default);
}
