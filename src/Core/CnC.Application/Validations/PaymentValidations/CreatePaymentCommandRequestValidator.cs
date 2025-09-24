using CnC.Application.Features.Payment;
using FluentValidation;

namespace CnC.Application.Validations.PaymentValidations;

public class CreatePaymentCommandRequestValidator : AbstractValidator<CreatePaymentCommandRequest>
{
    public CreatePaymentCommandRequestValidator()
    {
        RuleFor(P => P.OrderId)
            .NotEmpty()
            .WithMessage("OrderId cannot be null");

        RuleFor(P => P.PaymentId)
            .NotEmpty()
            .WithMessage("PaymentId cannot be null");
    }
}
