using OpenSync.Core.Entities;
using OpenSync.Core.Interfaces.Repositories;

namespace OpenSync.Application.Streams.Commands.CreateStream;

public class CreateStreamHandler : ICommandHandler<CreateStreamCommand, Common.Result<Guid>>
{
    private readonly IStreamRepository _repository;

    public CreateStreamHandler(IStreamRepository repository) => _repository = repository;

    public async Task<Common.Result<Guid>> Handle(CreateStreamCommand request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(request.UniqueName))
        {
            var existing = await _repository.GetByServiceAndNameAsync(request.ServiceId, request.UniqueName, cancellationToken);
            if (existing != null)
                return Common.Result.Failure<Guid>("DUPLICATE", $"Stream with name '{request.UniqueName}' already exists.");
        }

        var stream = new SyncStream(request.ServiceId, request.UniqueName, request.ExpiresAt);
        await _repository.AddAsync(stream, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return Common.Result.Success(stream.Id);
    }
}
