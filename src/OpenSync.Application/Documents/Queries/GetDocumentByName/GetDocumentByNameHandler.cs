using OpenSync.Core.Interfaces.Repositories;

namespace OpenSync.Application.Documents.Queries.GetDocumentByName;

public class GetDocumentByNameHandler : IQueryHandler<GetDocumentByNameQuery, Common.Result<SyncDocument>>
{
    private readonly IDocumentRepository _repository;

    public GetDocumentByNameHandler(IDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Common.Result<SyncDocument>> Handle(GetDocumentByNameQuery request, CancellationToken cancellationToken)
    {
        var document = await _repository.GetByServiceAndNameAsync(request.ServiceId, request.UniqueName, cancellationToken);
        if (document == null)
            return Common.Result.Failure<SyncDocument>("NOT_FOUND", $"Document '{request.UniqueName}' not found in service {request.ServiceId}.");

        return Common.Result.Success(document);
    }
}
