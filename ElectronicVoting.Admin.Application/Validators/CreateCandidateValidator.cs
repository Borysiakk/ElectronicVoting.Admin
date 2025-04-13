using ElectronicVoting.Admin.Application.Handlers.Commands.Candidate;
using FluentValidation;

namespace ElectronicVoting.Admin.Application.Validators;

public class CreateCandidateValidator : AbstractValidator<CreateCandidate>
{
    public CreateCandidateValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(50).WithMessage("Name cannot exceed 50 characters.");

        // Age must be a positive integer
        RuleFor(x => x.Age)
            .GreaterThan(0).WithMessage("Age must be a positive number.");

        // Party must not be empty
        RuleFor(x => x.Party)
            .NotEmpty().WithMessage("Party is required.");

        // Description is optional but has a max length limit
        RuleFor(x => x.Description)
            .MaximumLength(250).WithMessage("Description cannot exceed 250 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        // Social media fields have optional max length restrictions
        RuleFor(x => x.Instagram)
            .MaximumLength(100).WithMessage("Instagram URL cannot exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Instagram));

        RuleFor(x => x.Facebook)
            .MaximumLength(100).WithMessage("Facebook URL cannot exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Facebook));

        RuleFor(x => x.Twitter)
            .MaximumLength(100).WithMessage("Twitter URL cannot exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Twitter));

        RuleFor(x => x.Website)
            .MaximumLength(100).WithMessage("Website URL cannot exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Website));
    }
}