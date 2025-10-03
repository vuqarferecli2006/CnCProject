using CnC.Application.Features.ProductDescription.Commands.Create;
using FluentValidation;

namespace CnC.Application.Validations.ProductDescriptionValidations;

public class CreateProductDescriptionRequestValidator : AbstractValidator<CreateProductDescriptionRequest>
{
    private readonly string[] _allowedExtensions = { ".jpeg", ".jpg", ".png", ".webp" };
    private const long MaxFileSize = 5 * 1024 * 1024; 
    public CreateProductDescriptionRequestValidator()
    {
        RuleFor(Pr => Pr.ProductId)
                .NotEmpty()
                .WithMessage("ProductId cannot be empty");

        
        RuleFor(Pr => Pr.Description)
            .NotEmpty().WithMessage("Description is required.")
            .Must(d => !string.IsNullOrWhiteSpace(d))
                .WithMessage("Description cannot contain only whitespace.")
            .MinimumLength(20).WithMessage("Description must be at least 20 characters long.")
            .Matches(@"^[A-Za-z0-9\s.,!?'-]*$")
                .WithMessage("Description can only contain letters, numbers, spaces, and basic punctuation.");

        RuleFor(Pr => Pr.Model)
            .NotEmpty()
            .WithMessage("Model is required")
            .MaximumLength(300)
            .WithMessage("Description can be at most 300 characters long");

        RuleFor(Pr => Pr.ImageUrl)
                .NotNull()
                .WithMessage("ImageUrl is required")
                .Must(file => file.Length > 0)
                    .WithMessage("ImageUrl cannot be empty")
                .Must(file => file.Length <= MaxFileSize)
                    .WithMessage("ImageUrl size must not exceed 5 MB")
                .Must(file =>
                {
                    var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                    return _allowedExtensions.Contains(ext);
                })
                    .WithMessage("ImageUrl must be a .jpeg, .jpg, .png, or .webp file");

    }
}
