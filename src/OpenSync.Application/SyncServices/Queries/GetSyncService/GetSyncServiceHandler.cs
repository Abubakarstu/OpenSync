using OpenSync.Core.Interfaces.Repositories;

namespace OpenSync.Application.SyncServices.Queries.GetSyncService;

public class GetSyncServiceHandler : IQueryHandler<GetSyncServiceQuery, Common.Result<SyncService>>
{
    private readonly ISyncServiceRepository _repository;

    public GetSyncServiceHandler(ISyncServiceRepository repository)
    {
        _repository = repository;
    }

    public async Task<Common.Result<SyncService>> Handle(GetSyncServiceQuery request, CancellationToken cancellationToken)
    {
        var service = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (service == null)
            return Common.Result.Failure<SyncService>("NOT_FOUND", $"Service '{request.Id}' not found.");

        return Common.Result.Success(service);
    }
}
