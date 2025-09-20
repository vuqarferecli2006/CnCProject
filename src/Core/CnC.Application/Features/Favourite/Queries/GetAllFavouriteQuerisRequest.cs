using CnC.Application.Shared.Responses;
using CnC.Domain.Enums;
using MediatR;

namespace CnC.Application.Features.Favourite.Queries;

public class GetAllFavouriteQuerisRequest:IRequest<BaseResponse<List<FavouriteResponse>>>
{
    public Currency Currency { get; set; }=Currency.AZN;
}
