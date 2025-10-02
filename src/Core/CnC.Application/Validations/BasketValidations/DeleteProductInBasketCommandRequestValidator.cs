using CnC.Application.Features.Basket.Commands.Delete;
using FluentValidation;

namespace CnC.Application.Validations.BasketValidations;

public class DeleteProductInBasketCommandRequestValidator : AbstractValidator<DeleteProductInBasketCommandRequest>
{
    public DeleteProductInBasketCommandRequestValidator()
    {
        RuleFor(B => B.ProductId)
            .NotEmpty()
            .WithMessage("ProductId cannot be null");
    }
}
