using CnC.Application.Features.Category.SubCategory.Commands.Create;
using FluentValidation;

namespace CnC.Application.Validations.CategoryValidations.SubCategoryValidations;

public class CreateSubCategoryCommandRequestValidator : AbstractValidator<CreateSubCategoryCommandRequest>
{
    public CreateSubCategoryCommandRequestValidator()
    {
        RuleFor(Sctg => Sctg.ParentCategoryId)
            .NotEmpty()
            .WithMessage("ParentCategoryId cannot be null");

        RuleFor(Sctg => Sctg.Name)
            .NotEmpty()
            .WithMessage("Subcategory's name cannot be null")
            .MaximumLength(300)
            .WithMessage("Name can be at most 300 characters long");

        RuleFor(Sctg => Sctg.Description)
            .NotEmpty()
            .WithMessage("Subcategory's description cannot be null")
            .MaximumLength(1000)
            .WithMessage("Description can be at most 1000 characters long");
    }
}
