using CnC.Application.Features.Category.MainCategory.Commands.Delete;
using FluentValidation;

namespace CnC.Application.Validations.CategoryValidations.MainCategoryValidations;

public class DeleteMainCategoryCommandRequestValidator : AbstractValidator<DeleteMainCategoryCommandRequest>
{
    public DeleteMainCategoryCommandRequestValidator()
    {
        RuleFor(Mctg => Mctg.Id)
            .NotEmpty()
            .WithMessage("Id cannot be null");
    }
}
