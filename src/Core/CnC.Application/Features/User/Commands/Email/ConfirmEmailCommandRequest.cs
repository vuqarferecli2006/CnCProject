using CnC.Application.Shared.Responses;
using MediatR;

namespace CnC.Application.Features.User.Commands.Email;

public class ConfirmEmailCommandRequest: IRequest<BaseResponse<string>>
{
    public string UserId { get; set; } = null!;
    public string Token { get; set; } = null!;
}
