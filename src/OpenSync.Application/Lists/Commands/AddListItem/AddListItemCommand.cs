using MediatR;
using OpenSync.Application.Common.Abstractions;

namespace OpenSync.Application.Lists.Commands.AddListItem;

public record AddListItemCommand(Guid ListId, string Data, DateTime? ExpiresAt) : ICommand<Common.Result<Guid>>;
