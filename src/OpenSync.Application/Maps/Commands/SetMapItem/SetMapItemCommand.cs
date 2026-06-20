using MediatR;
using OpenSync.Application.Common.Abstractions;

namespace OpenSync.Application.Maps.Commands.SetMapItem;

public record SetMapItemCommand(Guid MapId, string Key, string Data, DateTime? ExpiresAt) : ICommand<Common.Result>;
