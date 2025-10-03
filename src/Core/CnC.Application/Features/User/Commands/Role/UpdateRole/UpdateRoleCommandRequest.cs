using CnC.Application.Shared.Responses;
using MediatR;

namespace CnC.Application.Features.User.Commands.Role.UpdateRole;

public class UpdateRoleCommandRequest:IRequest<BaseResponse<string?>>
{
    public string RoleId { get; set; } = null!;
    public string Name { get; init; } = null!;
    public List<string> Permissions { get; set; } = new();
}
