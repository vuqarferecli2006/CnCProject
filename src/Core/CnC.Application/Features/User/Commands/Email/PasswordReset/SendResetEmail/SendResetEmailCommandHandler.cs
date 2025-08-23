using CnC.Application.Abstracts.Services;
using CnC.Application.Shared.Responses;
using CnC.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Net;
using System.Text;
using System.Web;

namespace CnC.Application.Features.User.Commands.Email.PasswordReset.SendResetEmail;

public class SendResetEmailCommandHandler : IRequestHandler<SendResetEmailCommandRequest, BaseResponse<string>>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IEmailService _emailService;

    public SendResetEmailCommandHandler(UserManager<AppUser> userManager, IEmailService emailService)
    {
        _userManager = userManager;
        _emailService = emailService;
    }

    public async Task<BaseResponse<string>> Handle(SendResetEmailCommandRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return new("Email not found", false, HttpStatusCode.NotFound);

        var resetlink = await GetEmailResetConfirm(user);

        await _emailService.SendEmailAsync(new List<string> { user.Email }, "Reset Password", resetlink);
        return new("Reset link sent to your email", true, HttpStatusCode.OK);
    }
    private async Task<string> GetEmailResetConfirm(AppUser user)
    {
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        var link = $"https://localhost:7252/api/Users/ResetConfirmEmail?email={user.Email}&token={encodedToken}";
        Console.WriteLine("Reset Password Link : " + link);
        return link;
    }
}
