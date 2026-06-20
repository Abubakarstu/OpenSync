using MediatR;
using OpenSync.Application.Common.Abstractions;

namespace OpenSync.Application.Channels.Commands.CreateChannel;

public record CreateChannelCommand(Guid ServiceId, string? UniqueName, string? ChannelType, bool? IsPrivate, int? MaxMembers, DateTime? ExpiresAt) : ICommand<Common.Result<Guid>>;
