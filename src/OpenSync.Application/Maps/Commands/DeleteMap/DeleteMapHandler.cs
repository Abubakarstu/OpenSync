using OpenSync.Core.Interfaces.Repositories;

namespace OpenSync.Application.Maps.Commands.DeleteMap;

public class DeleteMapHandler : ICommandHandler<DeleteMapCommand, Common.Result>
{
    private readonly IMapRepository _repository;
    private readonly IMapItemRepository _mapItemRepository;

    public DeleteMapHandler(IMapRepository repository, IMapItemRepository mapItemRepository)
    {
        _repository = repository;
        _mapItemRepository = mapItemRepository;
    }

    public async Task<Common.Result> Handle(DeleteMapCommand request, CancellationToken cancellationToken)
    {
        var map = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (map == null)
            return Common.Result.Failure("NOT_FOUND", $"Map '{request.Id}' not found.");

        await _mapItemRepository.DeleteByMapIdAsync(request.Id, cancellationToken);
        await _repository.DeleteAsync(map, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return Common.Result.Success();
    }
}
