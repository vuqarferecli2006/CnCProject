using CnC.Application.Features.User.Commands.Email.PasswordReset.ResetPassword;
using FluentValidation;

namespace CnC.Application.Validations.UserValidations.EmailValidations.PasswordResetValidations;

public class ResetPasswordCommandRequestValidator : AbstractValidator<ResetPasswordCommandRequest>
{
    public ResetPasswordCommandRequestValidator()
    {
        RuleFor(Rp => Rp.Email)
                .NotEmpty()
                .WithMessage("Email is required")
                .EmailAddress()
                .WithMessage("Email format is invalid");

        RuleFor(Rp => Rp.Token)
            .NotEmpty()
            .WithMessage("Token is required");

        RuleFor(Rp => Rp.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters");
    }
}
