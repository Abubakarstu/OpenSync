using OpenSync.Core.Interfaces.Repositories;

namespace OpenSync.Application.Channels.Queries.ListChannelMembers;

public class ListChannelMembersHandler : IQueryHandler<ListChannelMembersQuery, Common.Result<PagedResult<ChannelMember>>>
{
    private readonly IChannelMemberRepository _repository;

    public ListChannelMembersHandler(IChannelMemberRepository repository) => _repository = repository;

    public async Task<Common.Result<PagedResult<ChannelMember>>> Handle(ListChannelMembersQuery request, CancellationToken cancellationToken)
    {
        request.PageRequest.EnsureValid();
        var members = await _repository.GetByChannelIdPaginatedAsync(request.ChannelId, request.PageRequest.Page * request.PageRequest.PageSize, request.PageRequest.PageSize, cancellationToken);
        var totalCount = await _repository.GetCountByChannelIdAsync(request.ChannelId, cancellationToken);

        return Common.Result.Success(new PagedResult<ChannelMember>(members, totalCount, request.PageRequest.Page, request.PageRequest.PageSize));
    }
}
