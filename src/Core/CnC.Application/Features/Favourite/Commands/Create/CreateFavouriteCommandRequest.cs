using CnC.Application.Shared.Responses;
using MediatR;

namespace CnC.Application.Features.Favourite.Commands.Create;

public class CreateFavouriteCommandRequest:IRequest<BaseResponse<string>>
{
    public Guid ProductId { get; set; }
}
