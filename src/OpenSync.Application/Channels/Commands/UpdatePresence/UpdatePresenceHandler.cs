using OpenSync.Core.Events;
using OpenSync.Core.Interfaces.Repositories;
using OpenSync.Core.Interfaces.Services;
using OpenSync.Core.ValueObjects;

namespace OpenSync.Application.Channels.Commands.UpdatePresence;

public class UpdatePresenceHandler : ICommandHandler<UpdatePresenceCommand, Common.Result>
{
    private readonly IChannelMemberRepository _memberRepository;
    private readonly IChannelRepository _channelRepository;
    private readonly IEventPublisher _eventPublisher;

    public UpdatePresenceHandler(IChannelMemberRepository memberRepository, IChannelRepository channelRepository, IEventPublisher eventPublisher)
    {
        _memberRepository = memberRepository;
        _channelRepository = channelRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task<Common.Result> Handle(UpdatePresenceCommand request, CancellationToken cancellationToken)
    {
        var channel = await _channelRepository.GetByIdAsync(request.ChannelId, cancellationToken);
        if (channel == null)
            return Common.Result.Failure("NOT_FOUND", $"Channel '{request.ChannelId}' not found.");

        var member = await _memberRepository.GetByChannelAndIdentityAsync(request.ChannelId, request.Identity, cancellationToken);
        if (member == null)
            return Common.Result.Failure("NOT_FOUND", $"Member '{request.Identity}' not found in channel.");

        member.UpdateMetadata(request.Metadata != null ? new JsonData(request.Metadata) : null);
        await _memberRepository.UpdateAsync(member, cancellationToken);
        await _memberRepository.SaveChangesAsync(cancellationToken);

        var domainEvent = new ChannelPresenceUpdatedEvent(request.ChannelId.ToString(), request.Identity, request.Metadata);
        await _eventPublisher.PublishAsync(domainEvent, cancellationToken);

        return Common.Result.Success();
    }
}
