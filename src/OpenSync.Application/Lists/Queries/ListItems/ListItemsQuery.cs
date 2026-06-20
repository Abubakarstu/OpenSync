using MediatR;
using OpenSync.Application.Common.Abstractions;
using OpenSync.Application.Common.Models;
using OpenSync.Core.Entities;

namespace OpenSync.Application.Lists.Queries.ListItems;

public record ListItemsQuery(Guid ListId, PageRequest PageRequest) : IQuery<Common.Result<PagedResult<SyncListItem>>>;
