using MediatR;
using OpenSync.Application.Common.Abstractions;

namespace OpenSync.Application.Streams.Commands.PublishMessage;

public record PublishMessageCommand(Guid StreamId, string Data, string? PublisherId) : ICommand<Common.Result>;
