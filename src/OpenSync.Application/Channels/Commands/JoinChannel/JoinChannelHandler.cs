using OpenSync.Core.Entities;
using OpenSync.Core.Events;
using OpenSync.Core.Interfaces.Repositories;
using OpenSync.Core.Interfaces.Services;
using OpenSync.Core.ValueObjects;

namespace OpenSync.Application.Channels.Commands.JoinChannel;

public class JoinChannelHandler : ICommandHandler<JoinChannelCommand, Common.Result>
{
    private readonly IChannelRepository _channelRepository;
    private readonly IChannelMemberRepository _memberRepository;
    private readonly IEventPublisher _eventPublisher;

    public JoinChannelHandler(IChannelRepository channelRepository, IChannelMemberRepository memberRepository, IEventPublisher eventPublisher)
    {
        _channelRepository = channelRepository;
        _memberRepository = memberRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task<Common.Result> Handle(JoinChannelCommand request, CancellationToken cancellationToken)
    {
        var channel = await _channelRepository.GetByIdAsync(request.ChannelId, cancellationToken);
        if (channel == null)
            return Common.Result.Failure("NOT_FOUND", $"Channel '{request.ChannelId}' not found.");

        if (channel.Attributes != null && channel.MemberCount >= channel.Attributes.MaxMembers)
            return Common.Result.Failure("QUOTA_EXCEEDED", $"Channel member limit ({channel.Attributes.MaxMembers}) reached.");

        var existingMember = await _memberRepository.GetByChannelAndIdentityAsync(request.ChannelId, request.Identity, cancellationToken);
        if (existingMember != null)
        {
            existingMember.UpdateMetadata(request.Metadata != null ? new JsonData(request.Metadata) : null);
            await _memberRepository.UpdateAsync(existingMember, cancellationToken);
            await _memberRepository.SaveChangesAsync(cancellationToken);
            return Common.Result.Success();
        }

        var metadata = request.Metadata != null ? new JsonData(request.Metadata) : null;
        var member = new ChannelMember(request.ChannelId, request.Identity, metadata);

        await _memberRepository.AddAsync(member, cancellationToken);
        channel.IncrementMemberCount();
        await _channelRepository.UpdateAsync(channel, cancellationToken);
        await _memberRepository.SaveChangesAsync(cancellationToken);

        var domainEvent = new ChannelMemberJoinedEvent(request.ChannelId.ToString(), request.Identity, request.Metadata);
        await _eventPublisher.PublishAsync(domainEvent, cancellationToken);

        return Common.Result.Success();
    }
}
