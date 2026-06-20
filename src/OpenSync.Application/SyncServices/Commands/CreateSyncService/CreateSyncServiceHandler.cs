using MediatR;
using OpenSync.Core.Entities;
using OpenSync.Core.Interfaces.Repositories;

namespace OpenSync.Application.SyncServices.Commands.CreateSyncService;

public class CreateSyncServiceHandler : ICommandHandler<CreateSyncServiceCommand, Common.Result<Guid>>
{
    private readonly ISyncServiceRepository _repository;

    public CreateSyncServiceHandler(ISyncServiceRepository repository)
    {
        _repository = repository;
    }

    public async Task<Common.Result<Guid>> Handle(CreateSyncServiceCommand request, CancellationToken cancellationToken)
    {
        var exists = await _repository.NameExistsAsync(request.Name, cancellationToken);
        if (exists)
            return Common.Result.Failure<Guid>("DUPLICATE", $"A service with name '{request.Name}' already exists.");

        var service = new SyncService(request.Name, request.Description, request.WebhookUrl, request.WebhookSecret);
        await _repository.AddAsync(service, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return Common.Result.Success(service.Id);
    }
}
