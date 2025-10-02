using CnC.Application.Shared.Responses;
using CnC.Domain.Enums;
using MediatR;

namespace CnC.Application.Features.Payment;

public class CreatePaymentCommandRequest:IRequest<BaseResponse<string>>
{
    public Guid OrderId { get; set; }
    public Guid PaymentId { get; set; }
    public Currency Currency { get; set; }
}
