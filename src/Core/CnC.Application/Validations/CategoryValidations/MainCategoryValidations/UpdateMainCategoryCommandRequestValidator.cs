using CnC.Application.Features.Category.MainCategory.Commands.Update;
using FluentValidation;

namespace CnC.Application.Validations.CategoryValidations.MainCategoryValidations;

public class UpdateMainCategoryCommandRequestValidator : AbstractValidator<UpdateMainCategoryCommandRequest>
{
    public UpdateMainCategoryCommandRequestValidator()
    {
        RuleFor(Mctg => Mctg.Id)
            .NotEmpty()
            .WithMessage("Id cannot be null");

        When(Mctg => Mctg.Name is not null, () =>
        {
            RuleFor(Mctg => Mctg.Name!)
                .MaximumLength(300)
                .WithMessage("Name can be at most 300 characters long");
        });

        When(Mctg => Mctg.Description is not null, () =>
        {
            RuleFor(Mctg => Mctg.Description!)
                .MaximumLength(1000)
                .WithMessage("Description can be at most 1000 characters long");
        });
    }
}
