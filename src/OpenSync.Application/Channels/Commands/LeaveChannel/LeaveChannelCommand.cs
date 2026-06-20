using MediatR;
using OpenSync.Application.Common.Abstractions;

namespace OpenSync.Application.Channels.Commands.LeaveChannel;

public record LeaveChannelCommand(Guid ChannelId, string Identity) : ICommand<Common.Result>;
