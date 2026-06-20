using OpenSync.Core.Interfaces.Repositories;

namespace OpenSync.Application.Lists.Queries.ListItems;

public class ListItemsHandler : IQueryHandler<ListItemsQuery, Common.Result<PagedResult<SyncListItem>>>
{
    private readonly IListItemRepository _repository;

    public ListItemsHandler(IListItemRepository repository) => _repository = repository;

    public async Task<Common.Result<PagedResult<SyncListItem>>> Handle(ListItemsQuery request, CancellationToken cancellationToken)
    {
        request.PageRequest.EnsureValid();
        var items = await _repository.GetByListIdAsync(request.ListId, request.PageRequest.Page * request.PageRequest.PageSize, request.PageRequest.PageSize, cancellationToken);
        var totalCount = await _repository.GetCountByListIdAsync(request.ListId, cancellationToken);

        return Common.Result.Success(new PagedResult<SyncListItem>(items, totalCount, request.PageRequest.Page, request.PageRequest.PageSize));
    }
}
