using Microsoft.EntityFrameworkCore;
using OpenSync.Core.Entities;
using OpenSync.Core.Interfaces.Repositories;

namespace OpenSync.Infrastructure.Persistence.Repositories;

public class ChannelMemberRepository : BaseRepository<ChannelMember>, IChannelMemberRepository
{
    public ChannelMemberRepository(OpenSyncDbContext context) : base(context) { }

    public async Task<ChannelMember?> GetByChannelAndIdentityAsync(Guid channelId, string identity, CancellationToken cancellationToken = default)
        => await DbSet.FirstOrDefaultAsync(m => m.ChannelId == channelId && m.Identity == identity, cancellationToken);

    public async Task<IReadOnlyList<ChannelMember>> GetByChannelIdAsync(Guid channelId, CancellationToken cancellationToken = default)
        => await DbSet.Where(m => m.ChannelId == channelId).ToListAsync(cancellationToken);

    public async Task<int> GetCountByChannelIdAsync(Guid channelId, CancellationToken cancellationToken = default)
        => await DbSet.CountAsync(m => m.ChannelId == channelId, cancellationToken);

    public async Task<IReadOnlyList<ChannelMember>> GetByChannelIdPaginatedAsync(Guid channelId, int skip = 0, int take = 50, CancellationToken cancellationToken = default)
        => await DbSet.Where(m => m.ChannelId == channelId).OrderBy(m => m.Identity).Skip(skip).Take(take).ToListAsync(cancellationToken);

    public async Task DeleteByChannelIdAsync(Guid channelId, CancellationToken cancellationToken = default)
    {
        var members = await DbSet.Where(m => m.ChannelId == channelId).ToListAsync(cancellationToken);
        DbSet.RemoveRange(members);
    }

    public async Task<IReadOnlyList<ChannelMember>> GetExpiredChannelMembersAsync(CancellationToken cancellationToken = default)
        => await DbSet.Where(m => m.LastSeenAt < DateTime.UtcNow.AddDays(-30)).ToListAsync(cancellationToken);
}
