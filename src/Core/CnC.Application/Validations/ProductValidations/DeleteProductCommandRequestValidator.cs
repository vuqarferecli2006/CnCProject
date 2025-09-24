using CnC.Application.Features.Product.Commands.Delete;
using FluentValidation;

namespace CnC.Application.Validations.ProductValidations;

public class DeleteProductCommandRequestValidator : AbstractValidator<DeleteProductCommandRequest>
{
    public DeleteProductCommandRequestValidator()
    {
        RuleFor(Pr => Pr.ProductId)
            .NotEmpty()
            .WithMessage("ProductId cannot be null");
    }
}
