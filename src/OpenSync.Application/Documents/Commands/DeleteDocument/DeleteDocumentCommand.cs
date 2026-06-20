using MediatR;
using OpenSync.Application.Common.Abstractions;

namespace OpenSync.Application.Documents.Commands.DeleteDocument;

public record DeleteDocumentCommand(Guid Id) : ICommand<Common.Result>;
