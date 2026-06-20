using MediatR;
using OpenSync.Application.Common.Abstractions;
using OpenSync.Core.Entities;

namespace OpenSync.Application.Streams.Queries.GetStream;

public record GetStreamQuery(Guid Id) : IQuery<Common.Result<SyncStream>>;
