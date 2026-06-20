using MediatR;
using OpenSync.Application.Common.Abstractions;

namespace OpenSync.Application.Lists.Commands.InsertListItem;

public record InsertListItemCommand(Guid ListId, int Index, string Data, DateTime? ExpiresAt) : ICommand<Common.Result<Guid>>;
