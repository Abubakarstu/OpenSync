using MediatR;
using OpenSync.Application.Common.Abstractions;
using OpenSync.Core.Entities;

namespace OpenSync.Application.Documents.Queries.GetDocument;

public record GetDocumentQuery(Guid Id) : IQuery<Common.Result<SyncDocument>>;
