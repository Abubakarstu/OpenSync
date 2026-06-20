using OpenSync.Core.Interfaces.Repositories;

namespace OpenSync.Application.Streams.Commands.DeleteStream;

public class DeleteStreamHandler : ICommandHandler<DeleteStreamCommand, Common.Result>
{
    private readonly IStreamRepository _repository;

    public DeleteStreamHandler(IStreamRepository repository) => _repository = repository;

    public async Task<Common.Result> Handle(DeleteStreamCommand request, CancellationToken cancellationToken)
    {
        var stream = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (stream == null)
            return Common.Result.Failure("NOT_FOUND", $"Stream '{request.Id}' not found.");

        await _repository.DeleteAsync(stream, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return Common.Result.Success();
    }
}
