using MediatR;
using OpenSync.Application.Common.Abstractions;
using OpenSync.Application.Common.Models;
using OpenSync.Core.Entities;

namespace OpenSync.Application.Maps.Queries.ListMapItems;

public record ListMapItemsQuery(Guid MapId, PageRequest PageRequest) : IQuery<Common.Result<PagedResult<SyncMapItem>>>;
