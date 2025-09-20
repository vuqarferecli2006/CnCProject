using CnC.Application.Shared.Responses;
using MediatR;

namespace CnC.Application.Features.Basket.Commands.Create;

public class AddBasketCommandRequest:IRequest<BaseResponse<string>>
{
    public Guid ProductId { get; set; }
}
