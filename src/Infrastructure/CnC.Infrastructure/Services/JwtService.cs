using CnC.Application.Abstracts.Services;
using CnC.Application.Shared.Responses;
using CnC.Application.Shared.Settings;
using CnC.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CnC.Infrastructure.Services;

public class JwtService : IJwtService
{
    private readonly JwtSetting _jwtSetting;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public JwtService(IOptions<JwtSetting> jwtSetting, 
                UserManager<AppUser> userManager, 
                RoleManager<IdentityRole> roleManager)
    {
        _jwtSetting = jwtSetting.Value;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<TokenResponse> GenerateJwttoken(AppUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtSetting.Key);
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email)
        };
        var roles = await _userManager.GetRolesAsync(user);
        foreach (var roleName in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, roleName));
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role is not null)
            {
                var roleClaims = await _roleManager.GetClaimsAsync(role);
                var rolePermissions = roleClaims
                    .Where(c => c.Type == "Permission")
                    .Distinct();
                foreach (var permission in rolePermissions)
                {
                    claims.Add(new Claim("Permission", permission.Value));
                }
            }
        }
        var now = DateTime.UtcNow;

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            NotBefore = now,
            Expires = now.AddMinutes(_jwtSetting.ExpiresInMinutes).AddSeconds(1),
            Issuer = _jwtSetting.Issuer,
            IssuedAt = now,
            Audience = _jwtSetting.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);
        var refreshToken = GenerateRefreshToken();
        var refreshTokenExpiryDate = DateTime.UtcNow.AddHours(2);
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = refreshTokenExpiryDate;
        await _userManager.UpdateAsync(user);
        return new TokenResponse
        {
            Token = jwt,
            ExpireDate = tokenDescriptor.Expires!.Value,
            RefreshToken = refreshToken,
            RefreshTokenExpireDate = refreshTokenExpiryDate
        };
    }
    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = false,
            ValidIssuer = _jwtSetting.Issuer,
            ValidAudience = _jwtSetting.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.Key))
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            if (securityToken is JwtSecurityToken jwtToken &&
                jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
            {
                return principal;
            }
        }
        catch (Exception)
        {

            return null;
        }

        return null;
    }
    private string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}
