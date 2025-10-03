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


        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .Must(d => !string.IsNullOrWhiteSpace(d))
                .WithMessage("Description cannot contain only whitespace.")
            .Matches(@"^[A-Za-z0-9\s.,!?'-]*$")
                .WithMessage("Description can only contain letters, numbers, spaces, and basic punctuation.");
    }
}
