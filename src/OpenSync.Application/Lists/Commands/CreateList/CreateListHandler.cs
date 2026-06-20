using OpenSync.Core.Entities;
using OpenSync.Core.Interfaces.Repositories;

namespace OpenSync.Application.Lists.Commands.CreateList;

public class CreateListHandler : ICommandHandler<CreateListCommand, Common.Result<Guid>>
{
    private readonly IListRepository _repository;

    public CreateListHandler(IListRepository repository) => _repository = repository;

    public async Task<Common.Result<Guid>> Handle(CreateListCommand request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(request.UniqueName))
        {
            var existing = await _repository.GetByServiceAndNameAsync(request.ServiceId, request.UniqueName, cancellationToken);
            if (existing != null)
                return Common.Result.Failure<Guid>("DUPLICATE", $"List with name '{request.UniqueName}' already exists.");
        }

        var list = new SyncList(request.ServiceId, request.UniqueName, request.ExpiresAt);
        await _repository.AddAsync(list, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return Common.Result.Success(list.Id);
    }
}
