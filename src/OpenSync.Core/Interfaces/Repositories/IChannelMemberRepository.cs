using OpenSync.Core.Entities;

namespace OpenSync.Core.Interfaces.Repositories;

public interface IChannelMemberRepository : IRepository<ChannelMember>
{
    Task<ChannelMember?> GetByChannelAndIdentityAsync(Guid channelId, string identity, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ChannelMember>> GetByChannelIdAsync(Guid channelId, CancellationToken cancellationToken = default);
    Task<int> GetCountByChannelIdAsync(Guid channelId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ChannelMember>> GetByChannelIdPaginatedAsync(Guid channelId, int skip = 0, int take = 50, CancellationToken cancellationToken = default);
    Task DeleteByChannelIdAsync(Guid channelId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ChannelMember>> GetExpiredChannelMembersAsync(CancellationToken cancellationToken = default);
}
