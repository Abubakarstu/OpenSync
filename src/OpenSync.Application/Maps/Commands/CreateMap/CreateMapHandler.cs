using OpenSync.Core.Entities;
using OpenSync.Core.Interfaces.Repositories;

namespace OpenSync.Application.Maps.Commands.CreateMap;

public class CreateMapHandler : ICommandHandler<CreateMapCommand, Common.Result<Guid>>
{
    private readonly IMapRepository _repository;

    public CreateMapHandler(IMapRepository repository) => _repository = repository;

    public async Task<Common.Result<Guid>> Handle(CreateMapCommand request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(request.UniqueName))
        {
            var existing = await _repository.GetByServiceAndNameAsync(request.ServiceId, request.UniqueName, cancellationToken);
            if (existing != null)
                return Common.Result.Failure<Guid>("DUPLICATE", $"Map with name '{request.UniqueName}' already exists.");
        }

        var map = new SyncMap(request.ServiceId, request.UniqueName, request.ExpiresAt);
        await _repository.AddAsync(map, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return Common.Result.Success(map.Id);
    }
}
