using OpenSync.Core.Interfaces.Repositories;

namespace OpenSync.Application.Channels.Queries.ListChannels;

public class ListChannelsHandler : IQueryHandler<ListChannelsQuery, Common.Result<PagedResult<SyncChannel>>>
{
    private readonly IChannelRepository _repository;

    public ListChannelsHandler(IChannelRepository repository) => _repository = repository;

    public async Task<Common.Result<PagedResult<SyncChannel>>> Handle(ListChannelsQuery request, CancellationToken cancellationToken)
    {
        request.PageRequest.EnsureValid();
        var channels = await _repository.GetByServiceIdAsync(request.ServiceId, request.PageRequest.Page * request.PageRequest.PageSize, request.PageRequest.PageSize, cancellationToken);
        var totalCount = await _repository.CountAsync(c => c.ServiceId == request.ServiceId, cancellationToken);

        return Common.Result.Success(new PagedResult<SyncChannel>(channels, totalCount, request.PageRequest.Page, request.PageRequest.PageSize));
    }
}
