using CnC.Application.Features.PaymentMethod;
using FluentValidation;

namespace CnC.Application.Validations.PaymentMethodValidations;

public class CreatePaymentMethodCommandRequestValidator : AbstractValidator<CreatePaymentMethodCommandRequest>
{
    public CreatePaymentMethodCommandRequestValidator()
    {
        RuleFor(PayM => PayM.Name)
            .MaximumLength(200)
            .WithMessage(" Name can be at most 200 characters long");

        RuleFor(PayM => PayM.Currency)
            .NotEmpty()
            .WithMessage("Currency cannot be null")
            .IsInEnum()
            .WithMessage("Currency value is invalid.");

        RuleFor(PayM => PayM.MethodForPayment)
            .NotEmpty()
            .WithMessage("Method for payment cannot be null")
            .IsInEnum()
            .WithMessage("MethodPayment value is invalid.");
    }
}
