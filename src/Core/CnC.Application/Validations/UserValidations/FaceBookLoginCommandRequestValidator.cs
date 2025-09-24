using CnC.Application.Features.User.Commands.FaceBook;
using FluentValidation;

namespace CnC.Application.Validations.UserValidations;

public class FaceBookLoginCommandRequestValidator : AbstractValidator<FaceBookLoginCommandRequest>
{
    public FaceBookLoginCommandRequestValidator()
    {
        RuleFor(Fl => Fl.AccessToken)
               .NotEmpty()
               .WithMessage("AccessToken is required");

        RuleFor(Fl => Fl.RoleId)
            .IsInEnum()
            .WithMessage("RoleId value is invalid");
    }
}
