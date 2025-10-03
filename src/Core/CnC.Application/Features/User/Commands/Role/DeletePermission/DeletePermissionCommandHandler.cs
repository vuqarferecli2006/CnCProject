using CnC.Application.Shared.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;
using static CnC.Application.Shared.Permissions.Permission;

namespace CnC.Application.Features.User.Commands.Role.DeletePermission;

public class DeletePermissionCommandHandler : IRequestHandler<DeletePermissionCommandRequest, BaseResponse<string>>
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public DeletePermissionCommandHandler(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<BaseResponse<string>> Handle(DeletePermissionCommandRequest request, CancellationToken cancellationToken)
    {
        if (request.Permissions is null || !request.Permissions.Any())
            return new("Permissions mustn't be empty", HttpStatusCode.BadRequest);

        var role = await _roleManager.FindByIdAsync(request.RoleId);
        if (role is null)
            return new("Role not found", false, HttpStatusCode.NotFound);

        var existingClaims = await _roleManager.GetClaimsAsync(role);
        var existingPermissionClaims = existingClaims.Where(c => c.Type == "Permission").ToList();

        var notFoundPermissions = request.Permissions
            .Where(p => !existingPermissionClaims.Any(ec => ec.Value == p))
            .ToList();

        if (notFoundPermissions.Any())
        {
            var joined = string.Join(", ", notFoundPermissions);
            return new($"The following permissions were not found on this role: {joined}", false, HttpStatusCode.BadRequest);
        }

        foreach (var permission in request.Permissions)
        {
            var claimToRemove = existingPermissionClaims.First(c => c.Value == permission);
            var result = await _roleManager.RemoveClaimAsync(role, claimToRemove);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new($"Failed to remove permission '{permission}': {errors}", false, HttpStatusCode.BadRequest);
            }
        }

        return new("Selected permissions removed successfully", true, HttpStatusCode.OK);

    }
}
