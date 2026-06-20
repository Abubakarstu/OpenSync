using OpenSync.Core.Events;
using OpenSync.Core.Interfaces.Repositories;
using OpenSync.Core.Interfaces.Services;

namespace OpenSync.Application.Channels.Commands.LeaveChannel;

public class LeaveChannelHandler : ICommandHandler<LeaveChannelCommand, Common.Result>
{
    private readonly IChannelRepository _channelRepository;
    private readonly IChannelMemberRepository _memberRepository;
    private readonly IEventPublisher _eventPublisher;

    public LeaveChannelHandler(IChannelRepository channelRepository, IChannelMemberRepository memberRepository, IEventPublisher eventPublisher)
    {
        _channelRepository = channelRepository;
        _memberRepository = memberRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task<Common.Result> Handle(LeaveChannelCommand request, CancellationToken cancellationToken)
    {
        var channel = await _channelRepository.GetByIdAsync(request.ChannelId, cancellationToken);
        if (channel == null)
            return Common.Result.Failure("NOT_FOUND", $"Channel '{request.ChannelId}' not found.");

        var member = await _memberRepository.GetByChannelAndIdentityAsync(request.ChannelId, request.Identity, cancellationToken);
        if (member == null)
            return Common.Result.Failure("NOT_FOUND", $"Member '{request.Identity}' not found in channel.");

        await _memberRepository.DeleteAsync(member, cancellationToken);
        channel.DecrementMemberCount();
        await _channelRepository.UpdateAsync(channel, cancellationToken);
        await _memberRepository.SaveChangesAsync(cancellationToken);

        var domainEvent = new ChannelMemberLeftEvent(request.ChannelId.ToString(), request.Identity);
        await _eventPublisher.PublishAsync(domainEvent, cancellationToken);

        return Common.Result.Success();
    }
}
