using MediatR;
using OpenSync.Application.Common.Abstractions;

namespace OpenSync.Application.Channels.Commands.JoinChannel;

public record JoinChannelCommand(Guid ChannelId, string Identity, string? Metadata) : ICommand<Common.Result>;
