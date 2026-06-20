using MediatR;
using OpenSync.Application.Common.Abstractions;

namespace OpenSync.Application.Documents.Commands.PatchDocument;

public record PatchDocumentCommand(
    Guid Id,
    string Data,
    long? ExpectedRevision
) : ICommand<Common.Result>;
