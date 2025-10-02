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
               .NotEmpty().WithMessage("Description is required.")
            .Must(d => !string.IsNullOrWhiteSpace(d))
                .WithMessage("Description cannot contain only whitespace.")
            .MinimumLength(20).WithMessage("Description must be at least 20 characters long.")
            .Matches(@"^[A-Za-z0-9\s.,!?'-]*$")
                .WithMessage("Description can only contain letters, numbers, spaces, and basic punctuation.");

        });
    }
}
