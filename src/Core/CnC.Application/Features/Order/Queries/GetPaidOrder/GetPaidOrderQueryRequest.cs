using CnC.Application.Shared.Responses;
using MediatR;

namespace CnC.Application.Features.Order.Queries.GetPaidOrder;

public class GetPaidOrderQueryRequest:IRequest<BaseResponse<List<GetPaidOrderResponse>>>
{

}
