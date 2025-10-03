using CnC.Application.Features.User.Commands.Role.AddPermision;
using CnC.Application.Features.User.Commands.Role.CreateRole;
using CnC.Application.Features.User.Commands.Role.DeletePermission;
using CnC.Application.Features.User.Commands.Role.DeleteRole;
using CnC.Application.Features.User.Commands.Role.Request;
using CnC.Application.Features.User.Commands.Role.UpdateRole;
using CnC.Application.Features.User.Queries.GetAllRoleWithPermission;
using CnC.Application.Shared.Permissions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CnC.WepApi.Controllers.Role
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public RolesController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleCommandRequest request)
        {
            var response = await _mediator.Send(request);
            return StatusCode(((int)response.StatusCode), response);
        }
        [HttpPost]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleCommandRequest request)
        {
            var response = await _mediator.Send(request);
            return StatusCode((int)response.StatusCode, response);
        }
        [HttpPost]
        public async Task<IActionResult> AddPermission([FromBody] AddPermsionCommandRequest request)
        {
            var response= await _mediator.Send(request);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost]
        [Authorize(Policy = Permission.Role.UpdateRole)]
        public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleCommandRequest request)
        {
            var response=await _mediator.Send(request);
            return StatusCode((int)response.StatusCode,response);
        }

        [HttpDelete]
        [Authorize(Policy =Permission.Role.DeleteRole)]
        public async Task<IActionResult> DeleteRole([FromBody]DeleteRoleCommandRequest request)
        {
            var response=await _mediator.Send(request);
            return StatusCode((int)response.StatusCode,response);
        }

        [HttpDelete]
        [Authorize(Policy =Permission.Role.DeletePermission)]
        public async Task<IActionResult> DeletePermission([FromBody] DeletePermissionCommandRequest request)
        {
            var response=await _mediator.Send(request);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet]
        [Authorize(Policy =Permission.Role.GetAllRoles)]
        public async Task<IActionResult> GetAllRoleWithPermissions()
        {
            var response = await _mediator.Send(new GetRolePermissionQueryRequest());
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
