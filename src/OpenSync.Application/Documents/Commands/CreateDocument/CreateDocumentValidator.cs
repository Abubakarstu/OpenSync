using FluentValidation;

namespace OpenSync.Application.Documents.Commands.CreateDocument;

public class CreateDocumentValidator : AbstractValidator<CreateDocumentCommand>
{
    public CreateDocumentValidator()
    {
        RuleFor(x => x.ServiceId).NotEmpty();
        RuleFor(x => x.Data).NotEmpty().WithMessage("Document data is required");
        RuleFor(x => x.UniqueName).MaximumLength(256).When(x => x.UniqueName != null);
    }
}
