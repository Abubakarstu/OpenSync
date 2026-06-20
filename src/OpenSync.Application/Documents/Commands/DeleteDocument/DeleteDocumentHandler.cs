using OpenSync.Core.Interfaces.Repositories;

namespace OpenSync.Application.Documents.Commands.DeleteDocument;

public class DeleteDocumentHandler : ICommandHandler<DeleteDocumentCommand, Common.Result>
{
    private readonly IDocumentRepository _repository;

    public DeleteDocumentHandler(IDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Common.Result> Handle(DeleteDocumentCommand request, CancellationToken cancellationToken)
    {
        var document = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (document == null)
            return Common.Result.Failure("NOT_FOUND", $"Document '{request.Id}' not found.");

        await _repository.DeleteAsync(document, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return Common.Result.Success();
    }
}
