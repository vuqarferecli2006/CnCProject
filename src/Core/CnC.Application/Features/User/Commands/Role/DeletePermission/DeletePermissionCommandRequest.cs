using CnC.Application.Shared.Responses;
using MediatR;

namespace CnC.Application.Features.User.Commands.Role.DeletePermission;

public class DeletePermissionCommandRequest:IRequest<BaseResponse<string>>
{
    public string RoleId { get; set; } = null!;
    public List<string> Permissions { get; set;} = new List<string>();
}
