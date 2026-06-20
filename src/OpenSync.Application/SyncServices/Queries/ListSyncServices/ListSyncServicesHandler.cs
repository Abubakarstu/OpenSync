using OpenSync.Core.Interfaces.Repositories;

namespace OpenSync.Application.SyncServices.Queries.ListSyncServices;

public class ListSyncServicesHandler : IQueryHandler<ListSyncServicesQuery, Common.Result<PagedResult<SyncService>>>
{
    private readonly ISyncServiceRepository _repository;

    public ListSyncServicesHandler(ISyncServiceRepository repository)
    {
        _repository = repository;
    }

    public async Task<Common.Result<PagedResult<SyncService>>> Handle(ListSyncServicesQuery request, CancellationToken cancellationToken)
    {
        request.PageRequest.EnsureValid();
        var services = await _repository.GetAllAsync(cancellationToken);
        var totalCount = services.Count;
        var paged = services
            .Skip(request.PageRequest.Page * request.PageRequest.PageSize)
            .Take(request.PageRequest.PageSize)
            .ToList();

        return Common.Result.Success(new PagedResult<SyncService>(paged, totalCount, request.PageRequest.Page, request.PageRequest.PageSize));
    }
}
