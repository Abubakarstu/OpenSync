using MediatR;
using OpenSync.Application.Common.Abstractions;
using OpenSync.Core.Entities;

namespace OpenSync.Application.Maps.Queries.GetMapItem;

public record GetMapItemQuery(Guid MapId, string Key) : IQuery<Common.Result<SyncMapItem>>;
