using OpenSync.Core.Interfaces.Repositories;

namespace OpenSync.Application.Maps.Queries.GetMapItem;

public class GetMapItemHandler : IQueryHandler<GetMapItemQuery, Common.Result<SyncMapItem>>
{
    private readonly IMapItemRepository _repository;

    public GetMapItemHandler(IMapItemRepository repository) => _repository = repository;

    public async Task<Common.Result<SyncMapItem>> Handle(GetMapItemQuery request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetByMapIdAndKeyAsync(request.MapId, request.Key, cancellationToken);
        if (item == null)
            return Common.Result.Failure<SyncMapItem>("NOT_FOUND", $"Map item with key '{request.Key}' not found.");
        return Common.Result.Success(item);
    }
}
