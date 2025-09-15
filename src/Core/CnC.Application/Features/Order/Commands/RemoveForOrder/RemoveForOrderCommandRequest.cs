using CnC.Application.Shared.Responses;
using MediatR;

namespace CnC.Application.Features.Order.Commands.RemoveForOrder;

public class RemoveForOrderCommandRequest:IRequest<BaseResponse<string>>
{
   public Guid ProductId { get; set; }
}
