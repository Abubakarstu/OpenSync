using MediatR;
using OpenSync.Application.Common.Abstractions;
using OpenSync.Core.Entities;

namespace OpenSync.Application.SyncServices.Queries.GetSyncService;

public record GetSyncServiceQuery(Guid Id) : IQuery<Common.Result<SyncService>>;
