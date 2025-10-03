using CnC.Application.Abstracts.Services;
using CnC.Application.Shared.Responses;
using CnC.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Security.Claims;

namespace CnC.Application.Features.User.Commands.Profil.Image;

public class ProfilImageUpdateHandler : IRequestHandler<ProfilImageUpdateRequest, BaseResponse<string>>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IFileServices _cloudinaryService;

    public ProfilImageUpdateHandler(
        UserManager<AppUser> userManager,
        IHttpContextAccessor httpContextAccessor,
        IFileServices cloudinaryService)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _cloudinaryService = cloudinaryService;
    }

    public async Task<BaseResponse<string>> Handle(ProfilImageUpdateRequest request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return new("Unauthorized", HttpStatusCode.Unauthorized);

        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            return new("User not found", HttpStatusCode.NotFound);

        if (!string.IsNullOrEmpty(user.ProfilePictureUrl) &&
            user.ProfilePictureUrl.Contains("profile-picture-updated"))
        {
            await _cloudinaryService.DeleteFileAsync(user.ProfilePictureUrl);
        }

        string folderName = "profile-picture-updated";

        var uploadedImageUrl = await _cloudinaryService.UploadAsync(
            request.ProfilImage,
            folderName
        );

        user.ProfilePictureUrl = uploadedImageUrl;
        var updateResult = await _userManager.UpdateAsync(user);

        if (!updateResult.Succeeded)
        {
            var errors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
            return new(errors, HttpStatusCode.BadRequest);
        }

        return new("Profile picture updated successfully", uploadedImageUrl, true, HttpStatusCode.OK);
    }
}


