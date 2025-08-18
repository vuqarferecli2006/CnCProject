using CnC.Application.Shared.Responses;
using MediatR;

namespace CnC.Application.Features.User.Commands.Role.Request;

public class CreateRoleCommandRequest:IRequest<BaseResponse<string?>>
{
    public string RoleName { get; set; } = string.Empty;
    public List<string> Permissions { get; set; } = new ();
}
