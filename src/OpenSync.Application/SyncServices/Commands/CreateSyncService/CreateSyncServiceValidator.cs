using FluentValidation;

namespace OpenSync.Application.SyncServices.Commands.CreateSyncService;

public class CreateSyncServiceValidator : AbstractValidator<CreateSyncServiceCommand>
{
    public CreateSyncServiceValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Service name is required")
            .MaximumLength(128).WithMessage("Service name must not exceed 128 characters")
            .Matches(@"^[a-zA-Z0-9_-]+$").WithMessage("Service name can only contain letters, numbers, hyphens, and underscores");
    }
}
