using CnC.Application.Features.User.Commands.Login;
using FluentValidation;

namespace CnC.Application.Validations.UserValidations;

public class LoginUserCommandRequestValidator : AbstractValidator<LoginUserCommandRequest>
{
    public LoginUserCommandRequestValidator()
    {
        RuleFor(Ul => Ul.Email)
                .NotEmpty()
                .WithMessage("Email is required")
                .EmailAddress()
                .WithMessage("Email format is invalid");

        RuleFor(Ul => Ul.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters");
    }
}
