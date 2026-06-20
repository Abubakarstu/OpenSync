using MediatR;
using OpenSync.Application.Common.Abstractions;

namespace OpenSync.Application.Maps.Commands.RemoveMapItem;

public record RemoveMapItemCommand(Guid MapId, string Key) : ICommand<Common.Result>;
