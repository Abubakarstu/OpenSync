using MediatR;
using OpenSync.Application.Common.Abstractions;
using OpenSync.Application.Common.Models;
using OpenSync.Core.Entities;

namespace OpenSync.Application.Channels.Queries.ListChannels;

public record ListChannelsQuery(Guid ServiceId, PageRequest PageRequest) : IQuery<Common.Result<PagedResult<SyncChannel>>>;
