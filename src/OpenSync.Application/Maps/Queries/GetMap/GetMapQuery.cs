using MediatR;
using OpenSync.Application.Common.Abstractions;
using OpenSync.Core.Entities;

namespace OpenSync.Application.Maps.Queries.GetMap;

public record GetMapQuery(Guid Id) : IQuery<Common.Result<SyncMap>>;
