using CnC.Application.Features.User.Commands.Role.CreateRole;
using FluentValidation;

namespace CnC.Application.Validations.UserValidations.RoleValidations;

public class CreateRoleCommandRequestValidator : AbstractValidator<CreateRoleCommandRequest>
{
    public CreateRoleCommandRequestValidator()
    {
        RuleFor(Cr => Cr.RoleName)
            .NotEmpty()
            .WithMessage("Role name is required")
            .MaximumLength(100)
            .WithMessage("Role name can be at most 100 characters long");

        RuleFor(Cr => Cr.Permissions)
            .NotNull()
            .WithMessage("Permissions list cannot be null")
            .Must(list => list.Count > 0)
                .WithMessage("At least one permission must be specified");
    }
}
