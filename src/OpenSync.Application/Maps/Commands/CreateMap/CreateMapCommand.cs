using MediatR;
using OpenSync.Application.Common.Abstractions;

namespace OpenSync.Application.Maps.Commands.CreateMap;

public record CreateMapCommand(Guid ServiceId, string? UniqueName, DateTime? ExpiresAt) : ICommand<Common.Result<Guid>>;
