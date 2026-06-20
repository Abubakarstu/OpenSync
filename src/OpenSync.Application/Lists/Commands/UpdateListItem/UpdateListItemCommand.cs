using MediatR;
using OpenSync.Application.Common.Abstractions;

namespace OpenSync.Application.Lists.Commands.UpdateListItem;

public record UpdateListItemCommand(Guid Id, string Data, long? ExpectedRevision) : ICommand<Common.Result>;
