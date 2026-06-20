using Microsoft.EntityFrameworkCore;
using OpenSync.Core.Entities;
using OpenSync.Core.Interfaces.Repositories;

namespace OpenSync.Infrastructure.Persistence.Repositories;

public class ListItemRepository : BaseRepository<SyncListItem>, IListItemRepository
{
    public ListItemRepository(OpenSyncDbContext context) : base(context) { }

    public async Task<IReadOnlyList<SyncListItem>> GetByListIdAsync(Guid listId, int skip = 0, int take = 50, CancellationToken cancellationToken = default)
        => await DbSet.Where(i => i.ListId == listId).OrderBy(i => i.Index).Skip(skip).Take(take).ToListAsync(cancellationToken);

    public async Task<int> GetCountByListIdAsync(Guid listId, CancellationToken cancellationToken = default)
        => await DbSet.CountAsync(i => i.ListId == listId, cancellationToken);

    public async Task<SyncListItem?> GetByListIdAndIndexAsync(Guid listId, int index, CancellationToken cancellationToken = default)
        => await DbSet.FirstOrDefaultAsync(i => i.ListId == listId && i.Index == index, cancellationToken);

    public async Task<IReadOnlyList<SyncListItem>> GetItemsFromIndexAsync(Guid listId, int fromIndex, CancellationToken cancellationToken = default)
        => await DbSet.Where(i => i.ListId == listId && i.Index >= fromIndex).OrderBy(i => i.Index).ToListAsync(cancellationToken);

    public async Task DeleteByListIdAsync(Guid listId, CancellationToken cancellationToken = default)
    {
        var items = await DbSet.Where(i => i.ListId == listId).ToListAsync(cancellationToken);
        DbSet.RemoveRange(items);
    }
}
