using OpenSync.Core.Interfaces.Repositories;

namespace OpenSync.Application.Channels.Queries.GetChannel;

public class GetChannelHandler : IQueryHandler<GetChannelQuery, Common.Result<SyncChannel>>
{
    private readonly IChannelRepository _repository;

    public GetChannelHandler(IChannelRepository repository) => _repository = repository;

    public async Task<Common.Result<SyncChannel>> Handle(GetChannelQuery request, CancellationToken cancellationToken)
    {
        var channel = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (channel == null)
            return Common.Result.Failure<SyncChannel>("NOT_FOUND", $"Channel '{request.Id}' not found.");
        return Common.Result.Success(channel);
    }
}
