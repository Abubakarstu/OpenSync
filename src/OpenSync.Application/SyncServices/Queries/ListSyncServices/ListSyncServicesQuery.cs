using MediatR;
using OpenSync.Application.Common.Abstractions;
using OpenSync.Application.Common.Models;
using OpenSync.Core.Entities;

namespace OpenSync.Application.SyncServices.Queries.ListSyncServices;

public record ListSyncServicesQuery(PageRequest PageRequest) : IQuery<Common.Result<PagedResult<SyncService>>>;
