using CnC.Application.Abstracts.Services;
using CnC.Application.DTOs.FaceBook;
using CnC.Application.Shared.Responses;
using CnC.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace CnC.Application.Features.User.Commands.FaceBook;

public class FaceBookLoginCommandHandler : IRequestHandler<FaceBookLoginCommandRequest, AuthResponse>
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<FaceBookLoginCommandHandler> _logger;
    private readonly IJwtService _jwtService;

    public FaceBookLoginCommandHandler(IConfiguration configuration, 
                                    UserManager<AppUser> userManager, 
                                    ILogger<FaceBookLoginCommandHandler> logger, 
                                    IJwtService jwtService)
    {
        _configuration = configuration;
        _userManager = userManager;
        _logger = logger;
        _jwtService = jwtService;
    }

    public async Task<AuthResponse> Handle(FaceBookLoginCommandRequest request, CancellationToken cancellationToken)
    {
        var fbAppId = _configuration["Facebook:AppId"] ?? throw new InvalidOperationException("Facebook AppId configuration is missing.");
        var fbAppSecret = _configuration["Facebook:AppSecret"] ?? throw new InvalidOperationException("Facebook AppSecret configuration is missing.");

        var fbUserInfoUrl = $"https://graph.facebook.com/me?fields=id,first_name,last_name,name,email,picture.width(200).height(200)&access_token={request.AccessToken}";
        FacebookUserInfo fbUser;
       
        using (var httpClient = new HttpClient())
        {
            var response = await httpClient.GetAsync(fbUserInfoUrl);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Failed to retrieve Facebook user info: {StatusCode}", response.StatusCode);
                throw new UnauthorizedAccessException("Invalid Facebook access token.");
            }
            var content = await response.Content.ReadAsStringAsync();
            fbUser = JsonSerializer.Deserialize<FacebookUserInfo>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? throw new InvalidOperationException("Failed to deserialize Facebook user info.");
        }
        var email = string.IsNullOrWhiteSpace(fbUser.Email)
            ? $"fb_{fbUser.Id}@facebook.local"
            : fbUser.Email;

        var fullName=$"{fbUser.FirstName} {fbUser.LastName}".Trim();
        if (string.IsNullOrWhiteSpace(fullName))
            fullName = fbUser.Name ?? "Facebook User";

        var userLogin=await _userManager.FindByLoginAsync("Facebook", fbUser.Id);

        if (userLogin is null)
        {
            userLogin = await _userManager.FindByEmailAsync(email);

            if (userLogin is null)
            {
                userLogin = new AppUser
                {
                    UserName = email,
                    Email = email,
                    FullName = fullName,
                    EmailConfirmed = true,
                    ProfilePictureUrl = fbUser.Picture.Data.Url
                };

                var createResult = await _userManager.CreateAsync(userLogin);
                if (!createResult.Succeeded)
                {
                    var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                    _logger.LogError("User creation failed for email {Email}: {Errors}", email, errors);
                    throw new InvalidOperationException($"Failed to create user: {errors}");
                }

                // ✅ Rol əlavə yalnız yeni user üçün
                if (request.RoleId is not null)
                {
                    var roleName = request.RoleId.ToString();
                    var addRoleResult = await _userManager.AddToRoleAsync(userLogin, roleName);
                    if (!addRoleResult.Succeeded)
                    {
                        var roleErrors = string.Join(", ", addRoleResult.Errors.Select(e => e.Description));
                        _logger.LogError("Assigning role {Role} to Facebook user {Email} failed: {Errors}", roleName, fbUser.Email, roleErrors);
                        throw new InvalidOperationException($"Failed to assign role: {roleErrors}");
                    }
                }
            }

            var addLoginResult = await _userManager.AddLoginAsync(userLogin, new UserLoginInfo("Facebook", fbUser.Id, "Facebook"));
            if (!addLoginResult.Succeeded)
            {
                var loginErrors = string.Join(", ", addLoginResult.Errors.Select(e => e.Description));
                _logger.LogError("Adding Facebook login failed for user {Email}: {Errors}", email, loginErrors);
                throw new InvalidOperationException($"Failed to add Facebook login info: {loginErrors}");
            }
        }

        else
        {
            // Profil şəkili yenilə
            if (!string.Equals(userLogin.ProfilePictureUrl, fbUser.Picture?.Data?.Url, StringComparison.OrdinalIgnoreCase))
            {
                userLogin.ProfilePictureUrl = fbUser.Picture?.Data?.Url;
                await _userManager.UpdateAsync(userLogin);
            }
        }
        var token = await _jwtService.GenerateJwttoken(userLogin);
        var roles = await _userManager.GetRolesAsync(userLogin);

        return new AuthResponse
        {
            Token = token.Token,
            Expires = token.ExpireDate,
            FullName = userLogin.FullName,
            Email = userLogin.Email,
            ProfileImageUrl = userLogin.ProfilePictureUrl,
            Roles = roles.ToList(),
            RefreshToken = token.RefreshToken,
            RefreshTokenExpireDate = token.RefreshTokenExpireDate,
            StatusCode = HttpStatusCode.OK
        };

    }
}
