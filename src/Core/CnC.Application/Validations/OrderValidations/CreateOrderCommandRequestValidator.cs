using CnC.Application.Features.Order.Commands.Create;
using FluentValidation;

namespace CnC.Application.Validations.OrderValidations;

public class CreateOrderCommandRequestValidator : AbstractValidator<CreateOrderCommandRequest>
{
    public CreateOrderCommandRequestValidator()
    {
        RuleFor(O => O.BasketId)
            .NotEmpty()
            .WithMessage("BasketId cannot be null");
    }
}
