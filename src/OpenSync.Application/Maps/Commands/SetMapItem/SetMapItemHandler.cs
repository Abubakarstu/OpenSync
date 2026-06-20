using OpenSync.Core.Entities;
using OpenSync.Core.Interfaces.Repositories;
using OpenSync.Core.Interfaces.Services;
using OpenSync.Core.ValueObjects;

namespace OpenSync.Application.Maps.Commands.SetMapItem;

public class SetMapItemHandler : ICommandHandler<SetMapItemCommand, Common.Result>
{
    private readonly IMapItemRepository _repository;
    private readonly IMapRepository _mapRepository;
    private readonly IJsonValidator _jsonValidator;

    public SetMapItemHandler(IMapItemRepository repository, IMapRepository mapRepository, IJsonValidator jsonValidator)
    {
        _repository = repository;
        _mapRepository = mapRepository;
        _jsonValidator = jsonValidator;
    }

    public async Task<Common.Result> Handle(SetMapItemCommand request, CancellationToken cancellationToken)
    {
        var (isValid, error) = _jsonValidator.Validate(request.Data);
        if (!isValid)
            return Common.Result.Failure("INVALID_JSON", error);

        var map = await _mapRepository.GetByIdAsync(request.MapId, cancellationToken);
        if (map == null)
            return Common.Result.Failure("NOT_FOUND", $"Map '{request.MapId}' not found.");

        var limits = new Core.Entities.ServiceLimits();
        var existing = await _repository.GetByMapIdAndKeyAsync(request.MapId, request.Key, cancellationToken);
        var jsonData = new JsonData(request.Data, limits.MaxMapItemDataSizeBytes);

        if (existing != null)
        {
            existing.UpdateData(jsonData);
            await _repository.UpdateAsync(existing, cancellationToken);
        }
        else
        {
            if (map.ItemCount >= limits.MaxMapItems)
                return Common.Result.Failure("QUOTA_EXCEEDED", $"Map item limit ({limits.MaxMapItems}) reached.");

            var mapItem = new SyncMapItem(request.MapId, request.Key, jsonData, request.ExpiresAt);
            await _repository.AddAsync(mapItem, cancellationToken);
            map.AdjustItemCount(1);
        }

        map.IncrementRevision();
        await _mapRepository.UpdateAsync(map, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return Common.Result.Success();
    }
}
