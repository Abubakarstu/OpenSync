using OpenSync.Core.Events;
using OpenSync.Core.Interfaces.Repositories;
using OpenSync.Core.Interfaces.Services;
using OpenSync.Core.ValueObjects;

namespace OpenSync.Application.Streams.Commands.PublishMessage;

public class PublishMessageHandler : ICommandHandler<PublishMessageCommand, Common.Result>
{
    private readonly IStreamRepository _repository;
    private readonly IEventPublisher _eventPublisher;
    private readonly IJsonValidator _jsonValidator;

    public PublishMessageHandler(IStreamRepository repository, IEventPublisher eventPublisher, IJsonValidator jsonValidator)
    {
        _repository = repository;
        _eventPublisher = eventPublisher;
        _jsonValidator = jsonValidator;
    }

    public async Task<Common.Result> Handle(PublishMessageCommand request, CancellationToken cancellationToken)
    {
        var (isValid, error) = _jsonValidator.Validate(request.Data);
        if (!isValid)
            return Common.Result.Failure("INVALID_JSON", error);

        var stream = await _repository.GetByIdAsync(request.StreamId, cancellationToken);
        if (stream == null)
            return Common.Result.Failure("NOT_FOUND", $"Stream '{request.StreamId}' not found.");

        var limits = new Core.Entities.ServiceLimits();
        new JsonData(request.Data, limits.MaxStreamMessageSizeBytes);

        var domainEvent = new StreamMessageEvent(request.StreamId.ToString(), request.Data, request.PublisherId);
        await _eventPublisher.PublishAsync(domainEvent, cancellationToken);

        return Common.Result.Success();
    }
}
