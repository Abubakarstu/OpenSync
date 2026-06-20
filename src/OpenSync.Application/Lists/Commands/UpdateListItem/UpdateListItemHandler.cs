using OpenSync.Core.Interfaces.Repositories;
using OpenSync.Core.Interfaces.Services;
using OpenSync.Core.ValueObjects;

namespace OpenSync.Application.Lists.Commands.UpdateListItem;

public class UpdateListItemHandler : ICommandHandler<UpdateListItemCommand, Common.Result>
{
    private readonly IListItemRepository _repository;
    private readonly IJsonValidator _jsonValidator;

    public UpdateListItemHandler(IListItemRepository repository, IJsonValidator jsonValidator)
    {
        _repository = repository;
        _jsonValidator = jsonValidator;
    }

    public async Task<Common.Result> Handle(UpdateListItemCommand request, CancellationToken cancellationToken)
    {
        var (isValid, error) = _jsonValidator.Validate(request.Data);
        if (!isValid)
            return Common.Result.Failure("INVALID_JSON", error);

        var item = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (item == null)
            return Common.Result.Failure("NOT_FOUND", $"List item '{request.Id}' not found.");

        try
        {
            var limits = new Core.Entities.ServiceLimits();
            var jsonData = new JsonData(request.Data, limits.MaxListItemDataSizeBytes);
            item.UpdateData(jsonData, request.ExpectedRevision);
            await _repository.UpdateAsync(item, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
            return Common.Result.Success();
        }
        catch (Core.Exceptions.ConflictException ex)
        {
            return Common.Result.Failure(ex.Code, ex.Message);
        }
    }
}
