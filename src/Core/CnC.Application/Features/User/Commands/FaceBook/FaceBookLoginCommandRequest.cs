using CnC.Application.Shared.Responses;
using CnC.Domain.Enums;
using MediatR;

namespace CnC.Application.Features.User.Commands.FaceBook;

public class FaceBookLoginCommandRequest:IRequest<AuthResponse>
{
    public string AccessToken { get; set; }=null!;

    public MarketPlaceRole? RoleId { get; set; }
}
