using OpenSync.Core.Interfaces.Repositories;

namespace OpenSync.Application.SyncServices.Commands.UpdateSyncService;

public class UpdateSyncServiceHandler : ICommandHandler<UpdateSyncServiceCommand, Common.Result>
{
    private readonly ISyncServiceRepository _repository;

    public UpdateSyncServiceHandler(ISyncServiceRepository repository)
    {
        _repository = repository;
    }

    public async Task<Common.Result> Handle(UpdateSyncServiceCommand request, CancellationToken cancellationToken)
    {
        var service = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (service == null)
            return Common.Result.Failure("NOT_FOUND", $"Service '{request.Id}' not found.");

        service.Update(request.Name, request.Description, request.WebhookUrl, request.WebhookSecret);
        await _repository.UpdateAsync(service, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return Common.Result.Success();
    }
}
