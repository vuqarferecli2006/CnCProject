using CnC.Application.Abstracts.Services;
using CnC.Application.Shared.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

namespace CnC.Application.Features.User.Commands.Logout;

public class LogoutUserCommandHandler : IRequestHandler<LogoutUserCommandRequest, BaseResponse<string>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITokenBlacklistService _tokenBlacklistService;
    public LogoutUserCommandHandler(IHttpContextAccessor httpContextAccessor, 
                                    ITokenBlacklistService tokenBlacklistService)
    {
        _httpContextAccessor = httpContextAccessor;
        _tokenBlacklistService = tokenBlacklistService;
    }
    public async Task<BaseResponse<string>> Handle(LogoutUserCommandRequest request, CancellationToken cancellationToken)
    {
        var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        if (string.IsNullOrWhiteSpace(token))
        {
            return new("Token not found in request",HttpStatusCode.BadRequest);
        }

        JwtSecurityTokenHandler tokenHandler = new();
        JwtSecurityToken? jwtToken;

        try
        {
            jwtToken = tokenHandler.ReadJwtToken(token);
        }
        catch (Exception)
        {
            return new("Invalid token", HttpStatusCode.BadRequest);
        }

        var expireDate = jwtToken.ValidTo;

        await _tokenBlacklistService.BlacklistTokenAsync(token, expireDate);

        return new("Logged out successfully", true, HttpStatusCode.OK);
    }
}
