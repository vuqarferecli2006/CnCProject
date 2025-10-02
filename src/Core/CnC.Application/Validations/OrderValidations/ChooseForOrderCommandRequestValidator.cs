using CnC.Application.Features.Order.Commands.ChooseForOrder;
using FluentValidation;

namespace CnC.Application.Validations.OrderValidations;

public class ChooseForOrderCommandRequestValidator : AbstractValidator<ChooseForOrderCommandRequest>
{
    public ChooseForOrderCommandRequestValidator()
    {
        RuleFor(O => O.ProductId)
            .NotEmpty()
            .WithMessage("ProductId cannot be null");
    }
}
