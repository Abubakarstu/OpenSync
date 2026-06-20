using OpenSync.Core.Entities;
using OpenSync.Core.Interfaces.Repositories;
using OpenSync.Core.Interfaces.Services;
using OpenSync.Core.ValueObjects;

namespace OpenSync.Application.Lists.Commands.AddListItem;

public class AddListItemHandler : ICommandHandler<AddListItemCommand, Common.Result<Guid>>
{
    private readonly IListItemRepository _repository;
    private readonly IListRepository _listRepository;
    private readonly IJsonValidator _jsonValidator;

    public AddListItemHandler(IListItemRepository repository, IListRepository listRepository, IJsonValidator jsonValidator)
    {
        _repository = repository;
        _listRepository = listRepository;
        _jsonValidator = jsonValidator;
    }

    public async Task<Common.Result<Guid>> Handle(AddListItemCommand request, CancellationToken cancellationToken)
    {
        var (isValid, error) = _jsonValidator.Validate(request.Data);
        if (!isValid)
            return Common.Result.Failure<Guid>("INVALID_JSON", error);

        var list = await _listRepository.GetByIdAsync(request.ListId, cancellationToken);
        if (list == null)
            return Common.Result.Failure<Guid>("NOT_FOUND", $"List '{request.ListId}' not found.");

        var limits = new Core.Entities.ServiceLimits();
        if (list.ItemCount >= limits.MaxListItems)
            return Common.Result.Failure<Guid>("QUOTA_EXCEEDED", $"List item limit ({limits.MaxListItems}) reached.");

        var jsonData = new JsonData(request.Data, limits.MaxListItemDataSizeBytes);
        var item = new SyncListItem(request.ListId, list.ItemCount, jsonData, request.ExpiresAt);

        await _repository.AddAsync(item, cancellationToken);
        list.AdjustItemCount(1);
        list.IncrementRevision();
        await _listRepository.UpdateAsync(list, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return Common.Result.Success(item.Id);
    }
}
