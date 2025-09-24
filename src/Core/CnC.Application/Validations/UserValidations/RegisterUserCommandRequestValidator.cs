using CnC.Application.Features.User.Commands.Register;
using FluentValidation;

namespace CnC.Application.Validations.UserValidations;

public class RegisterUserCommandRequestValidator : AbstractValidator<RegisterUserCommandRequest>
{
    public RegisterUserCommandRequestValidator()
    {
        RuleFor(Ur => Ur.FullName)
                .NotEmpty()
                .WithMessage("Full name is required")
                .MaximumLength(100)
                .WithMessage("Full name can be at most 100 characters long");

        RuleFor(Ur => Ur.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Invalid email format");

        RuleFor(Ur => Ur.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters")
            .Matches("[A-Z]")
            .WithMessage("Password must contain at least one uppercase letter")
            .Matches("[a-z]")
            .WithMessage("Password must contain at least one lowercase letter")
            .Matches("[^a-zA-Z0-9]")
            .WithMessage("Password must contain at least one special character");

        When(Ur => !string.IsNullOrWhiteSpace(Ur.ProfilPictureUrl), () =>
        {
            RuleFor(Ur => Ur.ProfilPictureUrl!)
                .MaximumLength(500)
                .WithMessage("Profile picture URL can be at most 500 characters long");
                
        });

        RuleFor(Ur => Ur.RoleId)
            .IsInEnum()
            .WithMessage("RoleId value is invalid");
    }
}
