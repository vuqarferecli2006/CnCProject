using CnC.Application.Features.Basket.Commands.Create;
using FluentValidation;

namespace CnC.Application.Validations.BasketValidations;

public class AddBasketCommandRequestValidator : AbstractValidator<AddBasketCommandRequest>
{
    public AddBasketCommandRequestValidator()
    {
        RuleFor(B => B.ProductId)
            .NotEmpty()
            .WithMessage("ProductId cannot be null");
    }
}
