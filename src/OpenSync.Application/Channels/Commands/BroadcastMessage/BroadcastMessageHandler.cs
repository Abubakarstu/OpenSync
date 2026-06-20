using OpenSync.Core.Events;
using OpenSync.Core.Interfaces.Repositories;
using OpenSync.Core.Interfaces.Services;

namespace OpenSync.Application.Channels.Commands.BroadcastMessage;

public class BroadcastMessageHandler : ICommandHandler<BroadcastMessageCommand, Common.Result>
{
    private readonly IChannelRepository _repository;
    private readonly IEventPublisher _eventPublisher;
    private readonly IJsonValidator _jsonValidator;

    public BroadcastMessageHandler(IChannelRepository repository, IEventPublisher eventPublisher, IJsonValidator jsonValidator)
    {
        _repository = repository;
        _eventPublisher = eventPublisher;
        _jsonValidator = jsonValidator;
    }

    public async Task<Common.Result> Handle(BroadcastMessageCommand request, CancellationToken cancellationToken)
    {
        var (isValid, error) = _jsonValidator.Validate(request.Data);
        if (!isValid)
            return Common.Result.Failure("INVALID_JSON", error);

        var channel = await _repository.GetByIdAsync(request.ChannelId, cancellationToken);
        if (channel == null)
            return Common.Result.Failure("NOT_FOUND", $"Channel '{request.ChannelId}' not found.");

        var domainEvent = new ChannelMessageBroadcastEvent(request.ChannelId.ToString(), request.Data, request.PublisherId);
        await _eventPublisher.PublishAsync(domainEvent, cancellationToken);

        return Common.Result.Success();
    }
}
