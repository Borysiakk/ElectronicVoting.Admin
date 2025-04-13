using ElectronicVoting.Admin.Application.Handlers.Commands.Authentication;
using FluentValidation;

namespace ElectronicVoting.Admin.Application.Validators;

public class LoginValidator: AbstractValidator<Login>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email is not valid.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters.");
    }
}