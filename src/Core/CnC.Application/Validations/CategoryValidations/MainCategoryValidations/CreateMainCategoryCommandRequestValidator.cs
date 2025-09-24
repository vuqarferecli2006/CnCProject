using CnC.Application.Features.Category.MainCategory.Commands.Create;
using FluentValidation;

namespace CnC.Application.Validations.CategoryValidations.MainCategoryValidations;

public class CreateMainCategoryCommandRequestValidator : AbstractValidator<CreateMainCategoryCommandRequest>
{
    public CreateMainCategoryCommandRequestValidator()
    {
        RuleFor(Mctg => Mctg.Name)
            .NotEmpty()
            .WithMessage("Main category's name cannot be null")
            .MaximumLength(300)
            .WithMessage("Name can be at most 300 characters long");

        RuleFor(Mctg => Mctg.Description)
            .NotEmpty()
            .WithMessage("Main category's description cannot be null")
            .MaximumLength(1000)
            .WithMessage("Description can be at most 1000 characters long");
    }
}
