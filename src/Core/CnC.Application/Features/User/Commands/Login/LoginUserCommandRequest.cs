using CnC.Application.Shared.Responses;
using MediatR;

namespace CnC.Application.Features.User.Commands.Login;

public class LoginUserCommandRequest:IRequest<BaseResponse<TokenResponse>>
{
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;
}
