using CnC.Application.Abstracts.Services;
using CnC.Application.Shared.Responses;
using CnC.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Security.Claims;

namespace CnC.Application.Features.User.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommandRequest, BaseResponse<TokenResponse>>
{
    private readonly IJwtService _jwtService;
    private readonly UserManager<AppUser> _userManager;

    public RefreshTokenCommandHandler(IJwtService jwtService,
                                        UserManager<AppUser> userManager)
    {
        _jwtService = jwtService;
        _userManager = userManager;
    }

    public async Task<BaseResponse<TokenResponse>> Handle(RefreshTokenCommandRequest request, CancellationToken cancellationToken)
    {
        var principal = _jwtService.GetPrincipalFromExpiredToken(request.AccessToken);

        if (principal is null)
            return new("Invalid token", false, HttpStatusCode.Unauthorized);

        var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var user = await _userManager.FindByIdAsync(userId);
        if(user is null)
            return new("User not found", false, HttpStatusCode.NotFound);
        if ( user.RefreshToken is null  || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime < DateTime.UtcNow)
            return new("Invalid refresh token", false, HttpStatusCode.Unauthorized);

        var refreshToken = await _jwtService.GenerateJwttoken(user);

        return new("Token refreshed successfully",refreshToken,true,HttpStatusCode.OK);
    }
}
