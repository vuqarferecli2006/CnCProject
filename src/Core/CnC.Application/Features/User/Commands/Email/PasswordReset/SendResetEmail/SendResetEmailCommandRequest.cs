using CnC.Application.Shared.Responses;
using MediatR;

namespace CnC.Application.Features.User.Commands.Email.PasswordReset.SendResetEmail;

public class SendResetEmailCommandRequest:IRequest<BaseResponse<string>>
{
    public string Email { get; set; } = null!;
}
