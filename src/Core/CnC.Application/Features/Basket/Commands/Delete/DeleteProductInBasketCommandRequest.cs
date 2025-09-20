using CnC.Application.Shared.Responses;
using MediatR;

namespace CnC.Application.Features.Basket.Commands.Delete;

public class DeleteProductInBasketCommandRequest:IRequest<BaseResponse<string>>
{
    public Guid ProductId { get; set; }
}
