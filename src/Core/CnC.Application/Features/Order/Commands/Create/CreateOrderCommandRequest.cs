using CnC.Application.Shared.Responses;
using MediatR;

namespace CnC.Application.Features.Order.Commands.Create;

public class CreateOrderCommandRequest:IRequest<BaseResponse<string>>
{
}
