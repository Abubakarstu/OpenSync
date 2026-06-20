using OpenSync.Core.Interfaces.Repositories;

namespace OpenSync.Application.Streams.Queries.GetStream;

public class GetStreamHandler : IQueryHandler<GetStreamQuery, Common.Result<SyncStream>>
{
    private readonly IStreamRepository _repository;

    public GetStreamHandler(IStreamRepository repository) => _repository = repository;

    public async Task<Common.Result<SyncStream>> Handle(GetStreamQuery request, CancellationToken cancellationToken)
    {
        var stream = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (stream == null)
            return Common.Result.Failure<SyncStream>("NOT_FOUND", $"Stream '{request.Id}' not found.");
        return Common.Result.Success(stream);
    }
}
