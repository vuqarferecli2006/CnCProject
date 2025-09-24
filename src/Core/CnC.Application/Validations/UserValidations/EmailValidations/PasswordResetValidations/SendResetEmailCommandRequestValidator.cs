using CnC.Application.Features.User.Commands.Email.PasswordReset.SendResetEmail;
using FluentValidation;

namespace CnC.Application.Validations.UserValidations.EmailValidations.PasswordResetValidations;

public class SendResetEmailCommandRequestValidator : AbstractValidator<SendResetEmailCommandRequest>
{
    public SendResetEmailCommandRequestValidator()
    {
        RuleFor(Sr => Sr.Email)
                .NotEmpty()
                .WithMessage("Email is required")
                .EmailAddress()
                .WithMessage("Email format is invalid");
    }
}
