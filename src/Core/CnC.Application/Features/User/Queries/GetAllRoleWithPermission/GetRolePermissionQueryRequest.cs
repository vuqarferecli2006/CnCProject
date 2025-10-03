using CnC.Application.Shared.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace CnC.Application.Features.User.Queries.GetAllRoleWithPermission;

public class GetRolePermissionQueryRequest:IRequest<BaseResponse<List<RoleWithPermissionsResponse>>>
{
}
