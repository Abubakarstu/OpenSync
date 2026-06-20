using OpenSync.Core.Interfaces.Repositories;

namespace OpenSync.Application.Maps.Queries.ListMapItems;

public class ListMapItemsHandler : IQueryHandler<ListMapItemsQuery, Common.Result<PagedResult<SyncMapItem>>>
{
    private readonly IMapItemRepository _repository;

    public ListMapItemsHandler(IMapItemRepository repository) => _repository = repository;

    public async Task<Common.Result<PagedResult<SyncMapItem>>> Handle(ListMapItemsQuery request, CancellationToken cancellationToken)
    {
        request.PageRequest.EnsureValid();
        var items = await _repository.GetByMapIdAsync(request.MapId, request.PageRequest.Page * request.PageRequest.PageSize, request.PageRequest.PageSize, cancellationToken);
        var totalCount = await _repository.GetCountByMapIdAsync(request.MapId, cancellationToken);

        return Common.Result.Success(new PagedResult<SyncMapItem>(items, totalCount, request.PageRequest.Page, request.PageRequest.PageSize));
    }
}
