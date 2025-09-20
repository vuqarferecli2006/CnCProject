using CnC.Application.Shared.Responses;
using MediatR;

namespace CnC.Application.Features.Payment;

public class CreatePaymentCommandRequest:IRequest<BaseResponse<string>>
{
    public Guid OrderId { get; set; }
    public Guid PaymentId { get; set; }
}
