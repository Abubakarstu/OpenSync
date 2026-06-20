using MediatR;
using OpenSync.Application.Common.Abstractions;
using OpenSync.Application.Common.Models;
using OpenSync.Core.Entities;

namespace OpenSync.Application.Documents.Queries.ListDocuments;

public record ListDocumentsQuery(Guid ServiceId, PageRequest PageRequest) : IQuery<Common.Result<PagedResult<SyncDocument>>>;
