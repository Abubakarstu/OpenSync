using OpenSync.Core.Interfaces.Repositories;

namespace OpenSync.Application.Documents.Queries.GetDocument;

public class GetDocumentHandler : IQueryHandler<GetDocumentQuery, Common.Result<SyncDocument>>
{
    private readonly IDocumentRepository _repository;

    public GetDocumentHandler(IDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Common.Result<SyncDocument>> Handle(GetDocumentQuery request, CancellationToken cancellationToken)
    {
        var document = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (document == null)
            return Common.Result.Failure<SyncDocument>("NOT_FOUND", $"Document '{request.Id}' not found.");

        return Common.Result.Success(document);
    }
}
