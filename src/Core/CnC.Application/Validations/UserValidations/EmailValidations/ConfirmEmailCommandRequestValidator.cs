using CnC.Application.Features.User.Commands.Email.ConfirmEmail;
using FluentValidation;

namespace CnC.Application.Validations.UserValidations.EmailValidations;

public class ConfirmEmailCommandRequestValidator : AbstractValidator<ConfirmEmailCommandRequest>
{
    public ConfirmEmailCommandRequestValidator()
    {
        RuleFor(ConfEm => ConfEm.UserId)
                .NotEmpty()
                .WithMessage("UserId is required");

        RuleFor(ConfEm => ConfEm.Token)
            .NotEmpty()
            .WithMessage("Token is required");
    }
}
