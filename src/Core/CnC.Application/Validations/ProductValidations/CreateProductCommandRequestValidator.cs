using CnC.Application.Features.Product.Commands.Create;
using FluentValidation;

namespace CnC.Application.Validations.ProductValidations;

public class CreateProductCommandRequestValidator : AbstractValidator<CreateProductCommandRequest>
{

    private readonly string[] _allowedExtensions = { ".jpeg", ".jpg", ".png", ".webp" };
    private const long MaxFileSize = 5 * 1024 * 1024;
    public CreateProductCommandRequestValidator()
    {
        RuleFor(Pr => Pr.Name)
            .NotEmpty()
            .WithMessage("Product name cannot be null")
            .MaximumLength(500)
            .WithMessage("Name can be at most 500 characters long");


        RuleFor(Pr => Pr.DiscountedPercent)
                .InclusiveBetween(0, 100)
                .WithMessage("DiscountedPercent must be between 0 and 100");

        RuleFor(Pr => Pr.PriceAzn)
            .NotEmpty()
            .WithMessage("Product's price must be required")
            .GreaterThan(0)
            .WithMessage("PriceAzn must be greater than 0");


        RuleFor(Pr => Pr.CategoryId)
                .NotEmpty()
                .WithMessage("CategoryId cannot be null");

        
    }
}
