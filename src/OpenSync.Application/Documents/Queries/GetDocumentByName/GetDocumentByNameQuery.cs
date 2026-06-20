using MediatR;
using OpenSync.Application.Common.Abstractions;
using OpenSync.Core.Entities;

namespace OpenSync.Application.Documents.Queries.GetDocumentByName;

public record GetDocumentByNameQuery(Guid ServiceId, string UniqueName) : IQuery<Common.Result<SyncDocument>>;
