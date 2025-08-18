using CnC.Application.Features.User.Commands.Role.Request;
using CnC.Application.Shared.Helpers.RoleHelpers;
using CnC.Application.Shared.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace CnC.Application.Features.User.Commands.Role.Handler;

public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommandRequest, BaseResponse<string?>>
{
    private readonly RoleCreationHelper _roleCreationHelper;
    private readonly RoleManager<IdentityRole> _roleManager;

    public CreateRoleCommandHandler(RoleCreationHelper roleCreationHelper,
                                 RoleManager<IdentityRole> roleManager)
    {
        _roleCreationHelper = roleCreationHelper;
        _roleManager = roleManager;
    }

    public async Task<BaseResponse<string?>> Handle(CreateRoleCommandRequest request, CancellationToken cancellationToken)
    {
        var existingRoleResponse = await _roleCreationHelper.CheckIfRoleExists(request.RoleName);
        if (existingRoleResponse is not null)
            return existingRoleResponse;

        var identityRole = new IdentityRole(request.RoleName);
        var createRoleResult = await _roleManager.CreateAsync(identityRole);

        if (!createRoleResult.Succeeded)
            return _roleCreationHelper.GenerateErrorResponse(createRoleResult.Errors);

        var permissionResult = await _roleCreationHelper.AddPermissionsToRole(identityRole, request.Permissions);
        if (permissionResult is not null)
            return permissionResult;

        return new BaseResponse<string?>("Role created successfully", true, HttpStatusCode.OK);
    }
    
}
