using CnC.Application.Shared.Helpers.RoleHelpers;
using CnC.Application.Shared.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace CnC.Application.Features.User.Commands.Role.UpdateRole;

public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommandRequest, BaseResponse<string?>>
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly RoleUpdateHelper _roleUpdateHelper;
    public UpdateRoleCommandHandler(RoleManager<IdentityRole> roleManager, RoleUpdateHelper roleUpdateHelper)
    {
        _roleManager = roleManager;
        _roleUpdateHelper = roleUpdateHelper;
    }

    public async Task<BaseResponse<string?>> Handle(UpdateRoleCommandRequest request, CancellationToken cancellationToken)
    {
        var existRole = await _roleManager.FindByIdAsync(request.RoleId);
        if (existRole is null)
            return new("Role not found", false, HttpStatusCode.NotFound);

        var permissionValidation = _roleUpdateHelper.CheckPermissionsValidity(request.Permissions);
        if (permissionValidation is not null)
            return permissionValidation;

        var updateNameResult = await _roleUpdateHelper.UpdateRoleName(existRole, request.Name);
        if (updateNameResult is not null)
            return updateNameResult;

        var replacePermissionsResult = await _roleUpdateHelper.ReplacePermissions(existRole, request.Permissions);
        if (replacePermissionsResult is not null)
            return replacePermissionsResult;

        return new("Role updated successfully", true, HttpStatusCode.OK);
    }
}
