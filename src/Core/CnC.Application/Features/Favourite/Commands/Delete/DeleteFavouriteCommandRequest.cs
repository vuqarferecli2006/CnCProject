using CnC.Application.Shared.Responses;
using MediatR;

namespace CnC.Application.Features.Favourite.Commands.Delete;

public class DeleteFavouriteCommandRequest:IRequest<BaseResponse<string>>
{
    public Guid ProductId { get; set; }
}
