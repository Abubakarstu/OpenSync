using MediatR;
using OpenSync.Application.Common.Abstractions;

namespace OpenSync.Application.Streams.Commands.DeleteStream;

public record DeleteStreamCommand(Guid Id) : ICommand<Common.Result>;
