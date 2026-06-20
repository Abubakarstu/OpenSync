using OpenSync.Core.Interfaces.Repositories;
using OpenSync.Core.Interfaces.Services;
using OpenSync.Core.ValueObjects;

namespace OpenSync.Application.Documents.Commands.PatchDocument;

public class PatchDocumentHandler : ICommandHandler<PatchDocumentCommand, Common.Result>
{
    private readonly IDocumentRepository _repository;
    private readonly IJsonValidator _jsonValidator;
    private readonly Core.Entities.ServiceLimits _limits;

    public PatchDocumentHandler(IDocumentRepository repository, IJsonValidator jsonValidator)
    {
        _repository = repository;
        _jsonValidator = jsonValidator;
        _limits = new Core.Entities.ServiceLimits();
    }

    public async Task<Common.Result> Handle(PatchDocumentCommand request, CancellationToken cancellationToken)
    {
        var (isValid, error) = _jsonValidator.Validate(request.Data, _limits.MaxDocumentSizeBytes);
        if (!isValid)
            return Common.Result.Failure("INVALID_JSON", error);

        var document = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (document == null)
            return Common.Result.Failure("NOT_FOUND", $"Document '{request.Id}' not found.");

        try
        {
            var existingData = document.Data.Deserialize<Dictionary<string, object?>>() ?? new();
            var patchData = new JsonData(request.Data).Deserialize<Dictionary<string, object?>>() ?? new();

            foreach (var kvp in patchData)
                existingData[kvp.Key] = kvp.Value;

            var mergedJson = new JsonData(System.Text.Json.JsonSerializer.Serialize(existingData), _limits.MaxDocumentSizeBytes);
            document.UpdateData(mergedJson, request.ExpectedRevision);

            await _repository.UpdateAsync(document, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            return Common.Result.Success();
        }
        catch (Core.Exceptions.ConflictException ex)
        {
            return Common.Result.Failure(ex.Code, ex.Message);
        }
    }
}
