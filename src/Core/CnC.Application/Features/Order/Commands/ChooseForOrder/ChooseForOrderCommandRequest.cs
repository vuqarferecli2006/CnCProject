using CnC.Application.Shared.Responses;
using MediatR;

namespace CnC.Application.Features.Order.Commands.ChooseForOrder;

public class ChooseForOrderCommandRequest:IRequest<BaseResponse<string>>
{
    public Guid ProductId { get; set; }
}
