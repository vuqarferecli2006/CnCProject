using CnC.Application.Features.User.Commands.Email.ChangePaswword;
using FluentValidation;

namespace CnC.Application.Validations.UserValidations.EmailValidations;

public class ChangePasswordCommandRequestValidator : AbstractValidator<ChangePasswordCommandRequest>
{
    public ChangePasswordCommandRequestValidator()
    {
        RuleFor(cp => cp.CurrentPassword)
                .NotEmpty()
                .WithMessage("Current password is required")
                .MinimumLength(6)
                .WithMessage("Current password must be at least 6 characters");

        RuleFor(cp => cp.NewPassword)
            .NotEmpty()
            .WithMessage("New password is required")
            .MinimumLength(6)
            .WithMessage("New password must be at least 6 characters")
            .NotEqual(cp => cp.CurrentPassword)
            .WithMessage("New password cannot be the same as current password")
            .Matches("[A-Z]")
            .WithMessage("Password must contain at least one uppercase letter")
            .Matches("[a-z]")
            .WithMessage("Password must contain at least one lowercase letter")
            .Matches("[^a-zA-Z0-9]")
            .WithMessage("Password must contain at least one special character");

        RuleFor(cp => cp.ConfirmNewPassword)
               .NotEmpty()
               .WithMessage("Confirm password is required")
               .Equal(cp => cp.NewPassword)
               .WithMessage("Confirm password must match the new password");
    }
}
