using CnC.Application.Shared.Responses;
using CnC.Domain.Enums;
using MediatR;

namespace CnC.Application.Features.ProductView.Queries;

public class GetUserProductViewsQueryRequest: IRequest<BaseResponse<List<ProductViewResponse>>>
{
    public Currency Currency { get; set; } = Currency.AZN;
}
