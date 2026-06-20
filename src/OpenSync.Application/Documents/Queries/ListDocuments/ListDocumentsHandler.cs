using OpenSync.Core.Interfaces.Repositories;

namespace OpenSync.Application.Documents.Queries.ListDocuments;

public class ListDocumentsHandler : IQueryHandler<ListDocumentsQuery, Common.Result<PagedResult<SyncDocument>>>
{
    private readonly IDocumentRepository _repository;

    public ListDocumentsHandler(IDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Common.Result<PagedResult<SyncDocument>>> Handle(ListDocumentsQuery request, CancellationToken cancellationToken)
    {
        request.PageRequest.EnsureValid();
        var documents = await _repository.GetByServiceIdAsync(request.ServiceId, request.PageRequest.Page * request.PageRequest.PageSize, request.PageRequest.PageSize, cancellationToken);
        var totalCount = await _repository.CountAsync(d => d.ServiceId == request.ServiceId, cancellationToken);

        return Common.Result.Success(new PagedResult<SyncDocument>(documents, totalCount, request.PageRequest.Page, request.PageRequest.PageSize));
    }
}
