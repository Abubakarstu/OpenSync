using MediatR;
using OpenSync.Application.Common.Abstractions;

namespace OpenSync.Application.Documents.Commands.CreateDocument;

public record CreateDocumentCommand(
    Guid ServiceId,
    string? UniqueName,
    string Data,
    long? ExpectedRevision,
    DateTime? ExpiresAt
) : ICommand<Common.Result<Guid>>;
