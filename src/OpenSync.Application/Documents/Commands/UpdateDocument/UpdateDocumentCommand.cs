using MediatR;
using OpenSync.Application.Common.Abstractions;

namespace OpenSync.Application.Documents.Commands.UpdateDocument;

public record UpdateDocumentCommand(
    Guid Id,
    string Data,
    long? ExpectedRevision
) : ICommand<Common.Result>;
