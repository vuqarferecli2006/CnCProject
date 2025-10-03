using CnC.Application.Shared.Responses;
using CnC.Domain.Enums;
using MediatR;

namespace CnC.Application.Features.PaymentMethod.Queries;

public class GetPaymentMethodQueryRequest:IRequest<BaseResponse<List<PaymentMethodResponse>>>
{
}
