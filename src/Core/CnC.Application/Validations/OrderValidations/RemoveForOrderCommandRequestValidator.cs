using CnC.Application.Features.Order.Commands.RemoveForOrder;
using FluentValidation;

namespace CnC.Application.Validations.OrderValidations;

public class RemoveForOrderCommandRequestValidator : AbstractValidator<RemoveForOrderCommandRequest>
{
    public RemoveForOrderCommandRequestValidator()
    {
        RuleFor(O => O.ProductId)
            .NotEmpty()
            .WithMessage("ProductId cannot be null");
    }
}
