using CnC.Application.Shared.Helpers.PermissionHelpers;
using CnC.Application.Shared.Responses;
using CnC.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Security.Claims;

namespace CnC.Application.Features.User.Commands.Role.AddPermision;

public class AddPermsionCommandHandler : IRequestHandler<AddPermsionCommandRequest, BaseResponse<string>>
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public AddPermsionCommandHandler(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<BaseResponse<string>> Handle(AddPermsionCommandRequest request, CancellationToken cancellationToken)
    {
        if (request.Permissions is null || !request.Permissions.Any())
            return new("Permissions mustn't be empty", HttpStatusCode.BadRequest);

        var role =await _roleManager.FindByIdAsync(request.RoleId);
        if(role is null)
            return new("Role not found",HttpStatusCode.NotFound);

        var permissionsList = PermissionHelper.GetPermissionList();
        var invalidPermission = request.Permissions
            .Where(p => !permissionsList.Contains(p, StringComparer.OrdinalIgnoreCase))
            .ToList(); ;

        if (invalidPermission.Any())
        {
            var invalids = string.Join(", ", invalidPermission);
            return new($"Invalid permissions: {invalids}", false, HttpStatusCode.BadRequest);
        }

        var existingClaims = await _roleManager.GetClaimsAsync(role);
        var existingPermissionValues = existingClaims
            .Where(c => c.Type == "Permission")
            .Select(c => c.Value)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var newPermissions = request.Permissions
            .Where(p => !existingPermissionValues.Contains(p))
            .ToList();

        foreach (var perm in newPermissions)
        {
            var result = await _roleManager.AddClaimAsync(role, new Claim("Permission", perm));
            if (!result.Succeeded)
            {
                var error = string.Join("; ", result.Errors.Select(e => e.Description));
                return new($"Failed to add permission '{perm}': {error}", false, HttpStatusCode.BadRequest);
            }
        }

        return new("Permissions added successfully", true, HttpStatusCode.OK);
    }
}