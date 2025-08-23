using CnC.Application.Shared.Responses;
using CnC.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Security.Claims;

namespace CnC.Application.Features.User.Commands.Email.ChangePaswword;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommandRequest, BaseResponse<string>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<AppUser> _userManager;

    public ChangePasswordCommandHandler(IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
    {
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }

    public async Task<BaseResponse<string>> Handle(ChangePasswordCommandRequest request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return new("User not found",HttpStatusCode.Unauthorized);

        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            return new("User not found",HttpStatusCode.NotFound);

        if (request.NewPassword != request.ConfirmNewPassword)
            return new("New password and confirm new password do not match",HttpStatusCode.BadRequest);

        if (request.CurrentPassword == request.NewPassword)
            return new("New password cannot be the same as the current password", HttpStatusCode.BadRequest);

        var checkPassword = await _userManager.CheckPasswordAsync(user, request.CurrentPassword);
        if (!checkPassword)
            return new("Current password is incorrect", HttpStatusCode.BadRequest);

        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new($"Failed to change password: {errors}", HttpStatusCode.BadRequest);
        }

        return new("Password changed successfully", true, HttpStatusCode.OK);
    }
}
