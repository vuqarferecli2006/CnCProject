using CnC.Application.Shared.Responses;
using CnC.Application.Shared.Responsesl;
using CnC.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Security.Claims;

namespace CnC.Application.Features.User.Queries.GetMyProfile;

public class GetProfileQueryHandler : IRequestHandler<GetProfileQueryRequest, BaseResponse<UserProfileResponse>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<AppUser> _userManager;

    public GetProfileQueryHandler(IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
    {
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }

    public async Task<BaseResponse<UserProfileResponse>> Handle(GetProfileQueryRequest request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return new("Unauthorized", HttpStatusCode.Unauthorized);

        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            return new("User not found", HttpStatusCode.NotFound);

        var roles = await _userManager.GetRolesAsync(user);

        var response = new UserProfileResponse
        {
            FullName = user.FullName, 
            Email = user.Email,
            Role = roles.FirstOrDefault() ?? "User",
            ProfilePicture = user.ProfilePictureUrl
        };

        return new("Success",response,true,HttpStatusCode.OK);
    }
}

