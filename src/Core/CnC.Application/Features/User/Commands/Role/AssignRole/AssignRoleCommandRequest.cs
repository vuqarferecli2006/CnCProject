using CnC.Application.Shared.Responses;
using MediatR;

namespace CnC.Application.Features.User.Commands.Role.Request;

public class AssignRoleCommandRequest:IRequest<BaseResponse<string?>>
{
    public Guid UserId { get; set; }
    public List<Guid> RoleId { get; set; } = new();
}
