using CnC.Application.Features.Category.SubCategory.Commands.Delete;
using FluentValidation;

namespace CnC.Application.Validations.CategoryValidations.SubCategoryValidations;

public class DeleteSubCategoryCommandRequestValidator : AbstractValidator<DeleteSubCategoryCommandRequest>
{
    public DeleteSubCategoryCommandRequestValidator()
    {
        RuleFor(Sctg => Sctg.Id)
            .NotEmpty()
            .WithMessage("Id cannot be null");
    }
}
