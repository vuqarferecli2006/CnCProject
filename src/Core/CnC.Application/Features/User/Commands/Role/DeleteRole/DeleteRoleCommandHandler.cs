using CnC.Application.Shared.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace CnC.Application.Features.User.Commands.Role.DeleteRole;

public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommandRequest, BaseResponse<string>>
{
    public RoleManager<IdentityRole> _roleManager;

    public DeleteRoleCommandHandler(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<BaseResponse<string>> Handle(DeleteRoleCommandRequest request, CancellationToken cancellationToken)
    {
        var role = await _roleManager.FindByIdAsync(request.RoleId);
        if (role is null)
        {
            return new("Role not found", false, HttpStatusCode.NotFound);
        }

        var result = await _roleManager.DeleteAsync(role);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new($"Failed to delete role: {errors}", false, HttpStatusCode.BadRequest);
        }

        return new("Role deleted successfully", true, HttpStatusCode.OK);
    }
}
