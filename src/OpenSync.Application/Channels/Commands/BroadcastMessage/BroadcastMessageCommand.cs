using MediatR;
using OpenSync.Application.Common.Abstractions;

namespace OpenSync.Application.Channels.Commands.BroadcastMessage;

public record BroadcastMessageCommand(Guid ChannelId, string Data, string? PublisherId) : ICommand<Common.Result>;
