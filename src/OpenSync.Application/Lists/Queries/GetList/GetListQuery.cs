using MediatR;
using OpenSync.Application.Common.Abstractions;
using OpenSync.Core.Entities;

namespace OpenSync.Application.Lists.Queries.GetList;

public record GetListQuery(Guid Id) : IQuery<Common.Result<SyncList>>;
