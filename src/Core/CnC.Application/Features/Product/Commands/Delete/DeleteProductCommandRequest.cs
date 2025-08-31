using CnC.Application.Shared.Responses;
using MediatR;

namespace CnC.Application.Features.Product.Commands.Delete;

public class DeleteProductCommandRequest:IRequest<BaseResponse<string>>
{
    public Guid ProductId { get; set; }    
}
