using CnC.Application.Features.User.Commands.Role.Request;
using FluentValidation;

namespace CnC.Application.Validations.UserValidations.RoleValidations;

public class AssignRoleCommandRequestValidator : AbstractValidator<AssignRoleCommandRequest>
{
    public AssignRoleCommandRequestValidator()
    {
        RuleFor(Ar => Ar.UserId)
                .NotEmpty()
                .WithMessage("UserId cannot be empty");

        RuleFor(Ar => Ar.RoleId)
            .NotNull()
            .WithMessage("RoleId list cannot be null")
            .Must(list => list.Count > 0)
                .WithMessage("At least one role must be assigned");
    }
}
