using CnC.Application.Abstracts.Services;
using CnC.Application.DTOs.Email;
using CnC.Application.Shared.Responses;
using CnC.Domain.Entities;
using CnC.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Text;
using System.Web;

namespace CnC.Application.Features.UserCreationByAdmin.Commands.RegisterUserByAdmin;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, BaseResponse<string>>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IEmailQueueService _mailService;

    public CreateUserCommandHandler(UserManager<AppUser> userManager, IEmailQueueService mailService)
    {
        _userManager = userManager;
        _mailService = mailService;
    }

    public async Task<BaseResponse<string>> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser is not null)
            return new("This email is already registered", HttpStatusCode.BadRequest);

        if (!Enum.IsDefined(typeof(PlatformRole), request.Role))
            return new("Invalid role", HttpStatusCode.BadRequest);

        string defaultProfilePicture = "https://res.cloudinary.com/dfi81qldi/image/upload/v1759333492/profile_w71q2q.png";
       
        var user = new AppUser
        {
            FullName = request.FullName,
            Email = request.Email,
            UserName = request.Email,
            ProfilePictureUrl = defaultProfilePicture
        };

        IdentityResult identityResult = await _userManager.CreateAsync(user, request.Password);
        if (!identityResult.Succeeded)
        {
            StringBuilder errorMessage = new();
            foreach (var error in identityResult.Errors)
            {
                errorMessage.AppendLine(error.Description);
            }
            return new(errorMessage.ToString(), HttpStatusCode.BadRequest);
        }

        string roleName = request.Role switch
        {
            PlatformRole.Moderator => "Moderator",
            PlatformRole.Admin => "Admin",
            _ => throw new ArgumentOutOfRangeException(nameof(request.Role), "Invalid user role")
        };

        var roleResult = await _userManager.AddToRoleAsync(user, roleName);
        if (!roleResult.Succeeded)
            return new("User created but role assignment failed", HttpStatusCode.InternalServerError);

        var emailConfirmLink = await GenerateEmailConfirmationLinkAsync(user);

        var emailMessage = new EmailMessageDto
        {
            To = new List<string> { user.Email },
            Subject = "Email Confirmation",
            Body = emailConfirmLink
        };

        await _mailService.PublishAsync(emailMessage);

        return new("User registered successfully. Confirmation link sent to email.", true, HttpStatusCode.OK);
    }

    private async Task<string> GenerateEmailConfirmationLinkAsync(AppUser user)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var encodedToken = HttpUtility.UrlEncode(token);
        return $"https://localhost:7252/api/Users/ConfirmEmail?token={encodedToken}&userId={user.Id}";
    }
}
