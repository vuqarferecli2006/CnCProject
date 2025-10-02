using CnC.Application.Features.User.Commands.Google;
using FluentValidation;

namespace CnC.Application.Validations.UserValidations;

public class GoogleLoginCommandRequestValidator : AbstractValidator<GoogleLoginCommandRequest>
{
    public GoogleLoginCommandRequestValidator()
    {
        RuleFor(Gl => Gl.IdToken)
               .NotEmpty()
               .WithMessage("IdToken is required");

        RuleFor(Gl => Gl.RoleId)
            .IsInEnum()
            .WithMessage("RoleId value is invalid");
    }
}
