using CnC.Application.Abstracts.Services;
using CnC.Application.Shared.Responses;
using CnC.Domain.Entities;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;

namespace CnC.Application.Features.User.Commands.Google;

public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommandRequest, AuthResponse>
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<GoogleLoginCommandHandler> _logger;
    private readonly IJwtService _jwtService;

    public GoogleLoginCommandHandler(IConfiguration configuration, 
                                UserManager<AppUser> userManager, 
                                ILogger<GoogleLoginCommandHandler> logger, 
                                IJwtService jwtService)
    {
        _configuration = configuration;
        _userManager = userManager;
        _logger = logger;
        _jwtService = jwtService;
    }

    public async Task<AuthResponse> Handle(GoogleLoginCommandRequest request, CancellationToken cancellationToken)
    {
        var googleClientId = _configuration["Google:ClientId"] ?? throw new ArgumentNullException("Google ClientId configuration is missing.");

        GoogleJsonWebSignature.Payload payload;

        try
        {
            payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { googleClientId }
            });
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Invalid Google ID token.");
            throw new UnauthorizedAccessException("Invalid Google ID token.");
        }

        var userLogininfo = await _userManager.FindByLoginAsync("Google", payload.Subject);
        if (userLogininfo is null)
        {
            userLogininfo = await _userManager.FindByEmailAsync(payload.Email);

            if (userLogininfo is null)
            {
                userLogininfo = new AppUser
                {
                    UserName = payload.Email,
                    Email = payload.Email,
                    FullName = payload.Name,
                    EmailConfirmed = true,
                    ProfilePictureUrl = payload.Picture,
                };

                var createResult = await _userManager.CreateAsync(userLogininfo);
                if (!createResult.Succeeded)
                {
                    var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                    _logger.LogError("User creation failed for email {Email}: {Errors}", payload.Email, errors);
                    throw new InvalidOperationException($"Failed to create user: {errors}");
                }
            }
            var addLoginResult = await _userManager.AddLoginAsync(userLogininfo, new UserLoginInfo("Google", payload.Subject, "Google"));

            if (!addLoginResult.Succeeded)
            {
                var loginErrors = string.Join(", ", addLoginResult.Errors.Select(e => e.Description));
                _logger.LogError("Adding Google login failed for user {Email}: {Errors}", payload.Email, loginErrors);
                throw new InvalidOperationException($"Failed to add Google login info: {loginErrors}");
            }

            if (request.RoleId is not null)
            {
                var roleName = request.RoleId.ToString();
                var addRoleResult = await _userManager.AddToRoleAsync(userLogininfo, roleName);
                if (!addRoleResult.Succeeded)
                {
                    var roleErrors = string.Join(", ", addRoleResult.Errors.Select(e => e.Description));
                    _logger.LogError("Assigning role {Role} to user {Email} failed: {Errors}", roleName, payload.Email, roleErrors);
                    throw new InvalidOperationException($"Failed to assign role: {roleErrors}");
                }
            }
        }
        else
        {
            if (!string.Equals(userLogininfo.ProfilePictureUrl, payload.Picture, StringComparison.OrdinalIgnoreCase))
            {
                userLogininfo.ProfilePictureUrl = payload.Picture;
                await _userManager.UpdateAsync(userLogininfo);
            }
        }
        var tokenResponse = await _jwtService.GenerateJwttoken(userLogininfo);
        var roles = await _userManager.GetRolesAsync(userLogininfo);
        return new AuthResponse
        {
            Token = tokenResponse.Token,
            Expires = tokenResponse.ExpireDate,
            FullName = userLogininfo.FullName,
            Email = userLogininfo.Email,
            ProfileImageUrl = userLogininfo.ProfilePictureUrl,
            Roles = roles.ToList(),
            RefreshToken = tokenResponse.RefreshToken,
            RefreshTokenExpireDate = tokenResponse.RefreshTokenExpireDate,
            StatusCode = HttpStatusCode.OK
        };
    }
}

