using OpenSync.Core.Interfaces.Repositories;

namespace OpenSync.Application.Lists.Commands.RemoveListItem;

public class RemoveListItemHandler : ICommandHandler<RemoveListItemCommand, Common.Result>
{
    private readonly IListItemRepository _repository;
    private readonly IListRepository _listRepository;

    public RemoveListItemHandler(IListItemRepository repository, IListRepository listRepository)
    {
        _repository = repository;
        _listRepository = listRepository;
    }

    public async Task<Common.Result> Handle(RemoveListItemCommand request, CancellationToken cancellationToken)
    {
        var list = await _listRepository.GetByIdAsync(request.ListId, cancellationToken);
        if (list == null)
            return Common.Result.Failure("NOT_FOUND", $"List '{request.ListId}' not found.");

        if (request.Index < 0 || request.Index >= list.ItemCount)
            return Common.Result.Failure("INVALID_INDEX", $"Index {request.Index} out of range. List has {list.ItemCount} items.");

        var item = await _repository.GetByListIdAndIndexAsync(request.ListId, request.Index, cancellationToken);
        if (item == null)
            return Common.Result.Failure("NOT_FOUND", $"No item at index {request.Index}.");

        var itemsToShift = await _repository.GetItemsFromIndexAsync(request.ListId, request.Index + 1, cancellationToken);
        await _repository.DeleteAsync(item, cancellationToken);

        foreach (var shiftItem in itemsToShift)
            shiftItem.SetIndex(shiftItem.Index - 1);

        list.AdjustItemCount(-1);
        list.IncrementRevision();
        await _listRepository.UpdateAsync(list, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return Common.Result.Success();
    }
}
