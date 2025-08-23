using CnC.Application.Shared.Responses;
using MediatR;

namespace CnC.Application.Features.User.Commands.Email.PasswordReset.ResetPassword;

public class ResetPasswordCommandRequest:IRequest<BaseResponse<string>>
{
    public string Email { get; set; } = null!;
    public string Token { get; set; } = null!;
    public string Password { get; set; } = null!;
}
