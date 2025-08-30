using CnC.Application.Features.User.Commands.Email;
using CnC.Application.Features.User.Commands.Email.ChangePaswword;
using CnC.Application.Features.User.Commands.Email.ConfirmEmail;
using CnC.Application.Features.User.Commands.Email.PasswordReset.ResetPassword;
using CnC.Application.Features.User.Commands.Email.PasswordReset.SendResetEmail;
using CnC.Application.Features.User.Commands.FaceBook;
using CnC.Application.Features.User.Commands.Google;
using CnC.Application.Features.User.Commands.Login;
using CnC.Application.Features.User.Commands.Logout;
using CnC.Application.Features.User.Commands.RefreshToken;
using CnC.Application.Features.User.Commands.Register;
using CnC.Application.Features.User.Queries.ResetConfirmEmail;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Web;

namespace CnC.WepApi.Controllers.User;

[Route("api/[controller]/[action]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpPost]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserCommandRequest request)
    {
        var response = await _mediator.Send(request);
        return StatusCode((int)response.StatusCode, response);
    }

    [HttpPost]
    public async Task<IActionResult> LoginUser([FromBody] LoginUserCommandRequest request)
    {
        var response = await _mediator.Send(request);
        return StatusCode((int)response.StatusCode, response);
    }

    [HttpPost]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommandRequest request)
    {
        var response = await _mediator.Send(request);
        return StatusCode((int)response.StatusCode, response);
    }

    [HttpGet]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
    {
        var command = new ConfirmEmailCommandRequest
        {
            UserId = userId,
            Token = token
        };
        var response = await _mediator.Send(command);
        return StatusCode((int)response.StatusCode, response);
    }

    [HttpPost]
    public async Task<IActionResult> Google([FromBody] GoogleLoginCommandRequest request)
    {
        var response = await _mediator.Send(request);
        return StatusCode((int)response.StatusCode, response);
    }

    [HttpPost]
    public async Task<IActionResult> FaceBook([FromBody] FaceBookLoginCommandRequest request)
    {
        var response = await _mediator.Send(request);
        return StatusCode((int)response.StatusCode, response);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Logout([FromBody] LogoutUserCommandRequest request)
    {
        var response = await _mediator.Send(request);
        return StatusCode((int)response.StatusCode, response);
    }
    
    [HttpPost]
    public async Task<IActionResult> SendResetEmail([FromBody] SendResetEmailCommandRequest request)
    {
        var response = await _mediator.Send(request);
        return StatusCode((int)response.StatusCode, response);
    }

    [HttpGet]
    public async Task<IActionResult> ResetConfirmEmail([FromQuery] string email, [FromQuery] string token)
    {
        var model = await _mediator.Send(new ResetConfirmEmailQueryRequest
        {
            Email = email,
            Token = token
        });
        return Ok(new { model });
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommandRequest request)
    {
        var response = await _mediator.Send(request);
        return StatusCode((int)response.StatusCode, response);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommandRequest request)
    {
        var response = await _mediator.Send(request);
        return StatusCode((int)response.StatusCode, response);
    }
}