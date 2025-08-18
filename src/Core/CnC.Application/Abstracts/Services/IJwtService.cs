using CnC.Application.Shared.Responses;
using CnC.Domain.Entities;
using System.Security.Claims;

namespace CnC.Application.Abstracts.Services;

public interface IJwtService
{
    Task<TokenResponse> GenerateJwttoken(AppUser user);
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}
