using MediatR;
using OpenSync.Application.Common.Abstractions;

namespace OpenSync.Application.Lists.Commands.RemoveListItem;

public record RemoveListItemCommand(Guid ListId, int Index) : ICommand<Common.Result>;
