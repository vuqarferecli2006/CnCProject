using CnC.Application.Abstracts.Services;
using CnC.Application.DTOs;
using CnC.Application.DTOs.Email;
using CnC.Application.Shared.Responses;
using CnC.Domain.Entities;
using CnC.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Text;
using System.Web;

namespace CnC.Application.Features.User.Commands.Register;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommandRequest, BaseResponse<string>>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IEmailQueueService _mailService;

    public RegisterUserCommandHandler(UserManager<AppUser> userManager, IEmailQueueService mailService)
    {
        _userManager = userManager;
        _mailService = mailService;
    }

    public async Task<BaseResponse<string>> Handle(RegisterUserCommandRequest request, CancellationToken cancellationToken)
    {
        var existingEmail = await _userManager.FindByEmailAsync(request.Email);
        if (existingEmail is not null)
            return new("This email is already registered", HttpStatusCode.BadRequest);

        string defaultProfilePicture = "https://res.cloudinary.com/dfi81qldi/image/upload/v1759333492/profile_w71q2q.png";

        var user = new AppUser
        {
            FullName = request.FullName,
            Email = request.Email,
            UserName = request.Email,
            ProfilePictureUrl = defaultProfilePicture
        };

        var identityResult = await _userManager.CreateAsync(user, request.Password);
        if (!identityResult.Succeeded)
        {
            StringBuilder errorMessage = new();
            foreach (var error in identityResult.Errors)
            {
                errorMessage.AppendLine(error.Description);
            }
            return new(errorMessage.ToString(), HttpStatusCode.BadRequest);
        }

        var roleResult = await _userManager.AddToRoleAsync(user, MarketPlaceRole.Buyer.ToString());
        if (!roleResult.Succeeded)
            return new("Failed to assign role to user", HttpStatusCode.BadRequest);

        var emailConfirmLink = await GetEmailConfirm(user);

        var emailMessage = new EmailMessageDto
        {
            To = new List<string> { user.Email },
            Subject = "Email Confirmation",
            Body = emailConfirmLink
        };

        await _mailService.PublishAsync(emailMessage);

        return new("User registered successfully, Link sent to your email.", user.Id, true, HttpStatusCode.Created);
    }

    private async Task<string> GetEmailConfirm(AppUser user)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var emailConfirmLink = $"https://localhost:7252/api/Users/ConfirmEmail?token={HttpUtility.UrlEncode(token)}&userId={user.Id}";
        return emailConfirmLink;
    }
}

