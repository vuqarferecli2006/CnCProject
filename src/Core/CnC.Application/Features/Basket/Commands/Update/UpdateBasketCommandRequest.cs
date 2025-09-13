using CnC.Application.Shared.Responses;
using MediatR;

namespace CnC.Application.Features.Basket.Commands.Update;

public class UpdateBasketCommandRequest:IRequest<BaseResponse<string>>
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
