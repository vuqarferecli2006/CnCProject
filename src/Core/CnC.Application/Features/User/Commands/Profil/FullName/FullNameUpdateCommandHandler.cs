using CnC.Application.Shared.Responses;
using CnC.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace CnC.Application.Features.User.Commands.Profil.FullName;

public class FullNameUpdateCommandHandler : IRequestHandler<FullNameUpdateCommandRequest, BaseResponse<string>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<AppUser> _userManager;

    public FullNameUpdateCommandHandler(IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
    {
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }

    public async Task<BaseResponse<string>> Handle(FullNameUpdateCommandRequest request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
            return new("Unauthorized",HttpStatusCode.Unauthorized);

        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            return new("User not found", HttpStatusCode.NotFound);

        user.FullName = request.FullName;
        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)   
        {
            StringBuilder errorMessage = new();
            foreach (var error in result.Errors)
            {
                errorMessage.AppendLine(error.Description);
            }
            return new(errorMessage.ToString(), HttpStatusCode.BadRequest);
        }

        return new("Full name updated successfully", true, HttpStatusCode.OK);

    }
}
