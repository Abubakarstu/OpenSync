using OpenSync.Core.Interfaces.Repositories;

namespace OpenSync.Application.Lists.Commands.DeleteList;

public class DeleteListHandler : ICommandHandler<DeleteListCommand, Common.Result>
{
    private readonly IListRepository _listRepository;
    private readonly IListItemRepository _listItemRepository;

    public DeleteListHandler(IListRepository listRepository, IListItemRepository listItemRepository)
    {
        _listRepository = listRepository;
        _listItemRepository = listItemRepository;
    }

    public async Task<Common.Result> Handle(DeleteListCommand request, CancellationToken cancellationToken)
    {
        var list = await _listRepository.GetByIdAsync(request.Id, cancellationToken);
        if (list == null)
            return Common.Result.Failure("NOT_FOUND", $"List '{request.Id}' not found.");

        await _listItemRepository.DeleteByListIdAsync(request.Id, cancellationToken);
        await _listRepository.DeleteAsync(list, cancellationToken);
        await _listRepository.SaveChangesAsync(cancellationToken);

        return Common.Result.Success();
    }
}
