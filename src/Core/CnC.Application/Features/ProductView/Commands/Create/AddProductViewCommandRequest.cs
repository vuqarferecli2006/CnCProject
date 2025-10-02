using CnC.Application.Shared.Responses;
using MediatR;

namespace CnC.Application.Features.ProductView.Commands.Create;

public class AddProductViewCommandRequest:IRequest<BaseResponse<string>>
{
    public Guid ProductId { get; set; }
}
