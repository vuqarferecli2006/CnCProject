using CnC.Application.Shared.Responses;
using CnC.Domain.Enums;
using MediatR;

namespace CnC.Application.Features.Basket.Queries.GetBasket;

public class GetBasketQueriesRequest:IRequest<BaseResponse<BasketResponse>>
{
    public Currency Currency { get; set; } = Currency.AZN;
}
