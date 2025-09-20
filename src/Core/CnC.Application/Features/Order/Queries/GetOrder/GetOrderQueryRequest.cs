using CnC.Application.Shared.Responses;
using CnC.Domain.Enums;
using MediatR;

namespace CnC.Application.Features.Order.Queries.GetOrder;

public class GetOrderQueryRequest:IRequest<BaseResponse<OrderResponse>>
{
    public Currency Currency { get; set; }=Currency.AZN;
}
