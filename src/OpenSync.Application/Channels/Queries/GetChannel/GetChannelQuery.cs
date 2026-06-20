using MediatR;
using OpenSync.Application.Common.Abstractions;
using OpenSync.Core.Entities;

namespace OpenSync.Application.Channels.Queries.GetChannel;

public record GetChannelQuery(Guid Id) : IQuery<Common.Result<SyncChannel>>;
