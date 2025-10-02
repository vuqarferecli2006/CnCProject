using CnC.Application.Shared.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace CnC.Application.Features.User.Queries.GetAllRoleWithPermission;

public class GetRolePermissionQueryHandler : IRequestHandler<GetRolePermissionQueryRequest, BaseResponse<List<RoleWithPermissionsResponse>>>
{
    private readonly RoleManager<IdentityRole> _roleManager;
    public GetRolePermissionQueryHandler(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }
    public async Task<BaseResponse<List<RoleWithPermissionsResponse>>> Handle(GetRolePermissionQueryRequest request, CancellationToken cancellationToken)
    {
        var roles = _roleManager.Roles.ToList();

        var result = new List<RoleWithPermissionsResponse>();

        foreach (var role in roles)
        {
            var claims = await _roleManager.GetClaimsAsync(role);
            var permissions = claims
                .Where(c => c.Type == "Permission")
                .Select(c => c.Value)
                .ToList();

            result.Add(new RoleWithPermissionsResponse
            {
                RoleId = role.Id,
                RoleName = role.Name,
                Permissions = permissions
            });
        }

        return new(result, true, HttpStatusCode.OK);
    }
}
