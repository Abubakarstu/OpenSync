using OpenSync.Core.Interfaces.Repositories;

namespace OpenSync.Application.Maps.Commands.RemoveMapItem;

public class RemoveMapItemHandler : ICommandHandler<RemoveMapItemCommand, Common.Result>
{
    private readonly IMapItemRepository _repository;
    private readonly IMapRepository _mapRepository;

    public RemoveMapItemHandler(IMapItemRepository repository, IMapRepository mapRepository)
    {
        _repository = repository;
        _mapRepository = mapRepository;
    }

    public async Task<Common.Result> Handle(RemoveMapItemCommand request, CancellationToken cancellationToken)
    {
        var map = await _mapRepository.GetByIdAsync(request.MapId, cancellationToken);
        if (map == null)
            return Common.Result.Failure("NOT_FOUND", $"Map '{request.MapId}' not found.");

        var item = await _repository.GetByMapIdAndKeyAsync(request.MapId, request.Key, cancellationToken);
        if (item == null)
            return Common.Result.Failure("NOT_FOUND", $"Map item with key '{request.Key}' not found.");

        await _repository.DeleteAsync(item, cancellationToken);
        map.AdjustItemCount(-1);
        map.IncrementRevision();
        await _mapRepository.UpdateAsync(map, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return Common.Result.Success();
    }
}
