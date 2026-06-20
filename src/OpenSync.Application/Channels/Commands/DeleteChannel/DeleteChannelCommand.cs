using MediatR;
using OpenSync.Application.Common.Abstractions;

namespace OpenSync.Application.Channels.Commands.DeleteChannel;

public record DeleteChannelCommand(Guid Id) : ICommand<Common.Result>;
