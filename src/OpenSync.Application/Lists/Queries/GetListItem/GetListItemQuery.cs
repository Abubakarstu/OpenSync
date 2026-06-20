using MediatR;
using OpenSync.Application.Common.Abstractions;
using OpenSync.Core.Entities;

namespace OpenSync.Application.Lists.Queries.GetListItem;

public record GetListItemQuery(Guid Id) : IQuery<Common.Result<SyncListItem>>;
