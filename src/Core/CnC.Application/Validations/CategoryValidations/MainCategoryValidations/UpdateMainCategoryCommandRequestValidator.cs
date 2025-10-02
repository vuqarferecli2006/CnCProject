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
             .NotEmpty().WithMessage("Description is required.")
            .Must(d => !string.IsNullOrWhiteSpace(d))
                .WithMessage("Description cannot contain only whitespace.")
            .MinimumLength(20).WithMessage("Description must be at least 20 characters long.")
            .Matches(@"^[A-Za-z0-9\s.,!?'-]*$");
        });
    }
}
