using OpenSync.Core.Interfaces.Repositories;

namespace OpenSync.Application.Lists.Queries.GetList;

public class GetListHandler : IQueryHandler<GetListQuery, Common.Result<SyncList>>
{
    private readonly IListRepository _repository;

    public GetListHandler(IListRepository repository) => _repository = repository;

    public async Task<Common.Result<SyncList>> Handle(GetListQuery request, CancellationToken cancellationToken)
    {
        var list = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (list == null)
            return Common.Result.Failure<SyncList>("NOT_FOUND", $"List '{request.Id}' not found.");
        return Common.Result.Success(list);
    }
}
