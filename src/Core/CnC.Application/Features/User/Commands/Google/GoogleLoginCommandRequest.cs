using CnC.Application.Shared.Responses;
using CnC.Domain.Enums;
using MediatR;

namespace CnC.Application.Features.User.Commands.Google;

public class GoogleLoginCommandRequest:IRequest<AuthResponse>
{
    public string IdToken { get; set; } = null!;

    public MarketPlaceRole? RoleId { get; set; }
}
