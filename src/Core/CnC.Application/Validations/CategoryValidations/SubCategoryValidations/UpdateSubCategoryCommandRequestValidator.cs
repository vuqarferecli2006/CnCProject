using CnC.Application.Features.Category.SubCategory.Commands.Update;
using FluentValidation;

namespace CnC.Application.Validations.CategoryValidations.SubCategoryValidations;

public class UpdateSubCategoryCommandRequestValidator : AbstractValidator<UpdateSubCategoryCommandRequest>
{
    public UpdateSubCategoryCommandRequestValidator()
    {
        RuleFor(Sctg => Sctg.Id)
            .NotEmpty()
            .WithMessage("Id cannot be null");

        When(Sctg => Sctg.Name is not null, () =>
        {
            RuleFor(Sctg => Sctg.Name!)
                .MaximumLength(300)
                .WithMessage("Name can be at most 300 characters long");
        });

        When(Sctg => Sctg.Description is not null, () =>
        {
            RuleFor(Sctg => Sctg.Description!)
                .MaximumLength(1000)
                .WithMessage("Description can be at most 1000 characters long");
        });
    }
}
