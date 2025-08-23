using CnC.Application.Shared.Responses;
using CnC.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Net;
using System.Text;
using System.Web;

namespace CnC.Application.Features.User.Commands.Email.PasswordReset.ResetPassword;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommandRequest, BaseResponse<string>>
{
    private readonly UserManager<AppUser> _userManager;

    public ResetPasswordCommandHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<BaseResponse<string>> Handle(ResetPasswordCommandRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return new("User not found", false, HttpStatusCode.NotFound);

        var decodedBytes = WebEncoders.Base64UrlDecode(request.Token);
        var decodedToken = Encoding.UTF8.GetString(decodedBytes);

        var result = await _userManager.ResetPasswordAsync(user, decodedToken, request.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new($"Failed to reset password: {errors}", false, HttpStatusCode.BadRequest);
        }
        return new("Password reset successfully", true, HttpStatusCode.OK);
    }
}

