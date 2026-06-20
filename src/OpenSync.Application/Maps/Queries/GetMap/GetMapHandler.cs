using OpenSync.Core.Interfaces.Repositories;

namespace OpenSync.Application.Maps.Queries.GetMap;

public class GetMapHandler : IQueryHandler<GetMapQuery, Common.Result<SyncMap>>
{
    private readonly IMapRepository _repository;

    public GetMapHandler(IMapRepository repository) => _repository = repository;

    public async Task<Common.Result<SyncMap>> Handle(GetMapQuery request, CancellationToken cancellationToken)
    {
        var map = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (map == null)
            return Common.Result.Failure<SyncMap>("NOT_FOUND", $"Map '{request.Id}' not found.");
        return Common.Result.Success(map);
    }
}
