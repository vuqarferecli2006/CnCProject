using CnC.Application.Features.User.Commands.Role.Request;
using CnC.Application.Shared.Responses;
using CnC.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace CnC.Application.Features.User.Commands.Role.AssignRole;

public class AssingRoleCommandHandler : IRequestHandler<AssignRoleCommandRequest, BaseResponse<string?>>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AssingRoleCommandHandler(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<BaseResponse<string?>> Handle(AssignRoleCommandRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
        {
            return new("User not found", false, HttpStatusCode.NotFound);
        }
        var seenRoleNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var roleNamesToAssign = new List<string>();
        foreach (var roleId in request.RoleId.Distinct())
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());

            if (role == null || string.IsNullOrWhiteSpace(role.Name))
            {
                return new($"Role with ID '{roleId}' is invalid or has no name", false, HttpStatusCode.BadRequest);
            }
            if (await _userManager.IsInRoleAsync(user, role.Name))
            {
                return new($"User already has the role '{role.Name}'", false, HttpStatusCode.BadRequest);
            }
            if (!seenRoleNames.Add(role.Name))
            {
                return new($"Duplicate role '{role.Name}' detected in request", false, HttpStatusCode.BadRequest);
            }

            roleNamesToAssign.Add(role.Name);
        }
        var currentRoles = await _userManager.GetRolesAsync(user);
        if (currentRoles.Any())
        {
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
            {
                var errors = string.Join(", ", removeResult.Errors.Select(e => e.Description));
                return new($"Failed to remove existing roles: {errors}", false, HttpStatusCode.InternalServerError);
            }
        }
        foreach (var roleName in roleNamesToAssign)
        {
            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new($"Failed to assign role '{roleName}': {errors}", false, HttpStatusCode.BadRequest);
            }
        }
        return new($"Roles updated successfully: {string.Join(", ", roleNamesToAssign)}",true,HttpStatusCode.OK);
    }
}
