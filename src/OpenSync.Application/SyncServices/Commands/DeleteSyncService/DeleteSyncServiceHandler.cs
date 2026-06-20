using OpenSync.Core.Interfaces.Repositories;

namespace OpenSync.Application.SyncServices.Commands.DeleteSyncService;

public class DeleteSyncServiceHandler : ICommandHandler<DeleteSyncServiceCommand, Common.Result>
{
    private readonly ISyncServiceRepository _repository;

    public DeleteSyncServiceHandler(ISyncServiceRepository repository)
    {
        _repository = repository;
    }

    public async Task<Common.Result> Handle(DeleteSyncServiceCommand request, CancellationToken cancellationToken)
    {
        var service = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (service == null)
            return Common.Result.Failure("NOT_FOUND", $"Service '{request.Id}' not found.");

        await _repository.DeleteAsync(service, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return Common.Result.Success();
    }
}
