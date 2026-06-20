using OpenSync.Core.Interfaces.Repositories;

namespace OpenSync.Application.Lists.Queries.GetListItem;

public class GetListItemHandler : IQueryHandler<GetListItemQuery, Common.Result<SyncListItem>>
{
    private readonly IListItemRepository _repository;

    public GetListItemHandler(IListItemRepository repository) => _repository = repository;

    public async Task<Common.Result<SyncListItem>> Handle(GetListItemQuery request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (item == null)
            return Common.Result.Failure<SyncListItem>("NOT_FOUND", $"List item '{request.Id}' not found.");
        return Common.Result.Success(item);
    }
}
