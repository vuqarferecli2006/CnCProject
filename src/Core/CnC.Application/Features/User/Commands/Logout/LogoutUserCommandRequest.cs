using CnC.Application.Shared.Responses;
using MediatR;

namespace CnC.Application.Features.User.Commands.Logout;

public class LogoutUserCommandRequest:IRequest<BaseResponse<string>>
{
}
