using CnC.Application.Shared.Responses;
using CnC.Domain.Enums;
using MediatR;

namespace CnC.Application.Features.User.Commands.Register;

public class RegisterUserCommandRequest:IRequest<BaseResponse<string>>
{
    public string FullName { get; set; } = null!;
    
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? ProfilPictureUrl { get; set; }

    public MarketPlaceRole RoleId { get; set; }
}
