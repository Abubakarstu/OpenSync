using MediatR;
using OpenSync.Application.Common.Abstractions;

namespace OpenSync.Application.Channels.Commands.UpdatePresence;

public record UpdatePresenceCommand(Guid ChannelId, string Identity, string? Metadata) : ICommand<Common.Result>;
