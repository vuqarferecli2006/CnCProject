using CnC.Application.Shared.Responses;
using CnC.Domain.Enums;
using MediatR;

namespace CnC.Application.Features.PaymentMethod;

public class CreatePaymentMethodCommandRequest:IRequest<BaseResponse<object>>
{
    public string? Name { get; set; }
    
    public Currency Currency { get; set; }

    public MethodForPayment MethodForPayment { get; set; }
}
