using MediatR;
using OpenSync.Application.Common.Abstractions;

namespace OpenSync.Application.Streams.Commands.CreateStream;

public record CreateStreamCommand(Guid ServiceId, string? UniqueName, DateTime? ExpiresAt) : ICommand<Common.Result<Guid>>;
