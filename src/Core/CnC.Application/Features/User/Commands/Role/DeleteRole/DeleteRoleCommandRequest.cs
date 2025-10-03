using CnC.Application.Shared.Responses;
using MediatR;

namespace CnC.Application.Features.User.Commands.Role.DeleteRole;

public class DeleteRoleCommandRequest:IRequest<BaseResponse<string>>
{
    public string RoleId { get; set; } = null!;
}
