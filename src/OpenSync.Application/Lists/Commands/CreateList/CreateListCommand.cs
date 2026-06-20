using MediatR;
using OpenSync.Application.Common.Abstractions;

namespace OpenSync.Application.Lists.Commands.CreateList;

public record CreateListCommand(Guid ServiceId, string? UniqueName, DateTime? ExpiresAt) : ICommand<Common.Result<Guid>>;
