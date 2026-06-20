using OpenSync.Core.Entities;
using OpenSync.Core.Interfaces.Repositories;
using OpenSync.Core.Interfaces.Services;
using OpenSync.Core.ValueObjects;

namespace OpenSync.Application.Documents.Commands.CreateDocument;

public class CreateDocumentHandler : ICommandHandler<CreateDocumentCommand, Common.Result<Guid>>
{
    private readonly IDocumentRepository _repository;
    private readonly IJsonValidator _jsonValidator;
    private readonly Core.Entities.ServiceLimits _limits;

    public CreateDocumentHandler(IDocumentRepository repository, IJsonValidator jsonValidator)
    {
        _repository = repository;
        _jsonValidator = jsonValidator;
        _limits = new Core.Entities.ServiceLimits();
    }

    public async Task<Common.Result<Guid>> Handle(CreateDocumentCommand request, CancellationToken cancellationToken)
    {
        var (isValid, error) = _jsonValidator.Validate(request.Data, _limits.MaxDocumentSizeBytes);
        if (!isValid)
            return Common.Result.Failure<Guid>("INVALID_JSON", error);

        if (!string.IsNullOrEmpty(request.UniqueName))
        {
            var existing = await _repository.GetByServiceAndNameAsync(request.ServiceId, request.UniqueName, cancellationToken);
            if (existing != null)
                return Common.Result.Failure<Guid>("DUPLICATE", $"Document with name '{request.UniqueName}' already exists in this service.");
        }

        var jsonData = new JsonData(request.Data, _limits.MaxDocumentSizeBytes);
        var document = new SyncDocument(request.ServiceId, jsonData, request.UniqueName, request.ExpiresAt);

        await _repository.AddAsync(document, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return Common.Result.Success(document.Id);
    }
}
