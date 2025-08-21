using System.Net;

namespace CnC.Application.Shared.Responses;

public class AuthResponse
{
    public HttpStatusCode StatusCode { get; set; }
    public string Token { get; set; } = null!;
    public DateTime Expires { get; set; }
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? ProfileImageUrl { get; set; }
    public List<string> Roles { get; set; } = new();
    public string RefreshToken { get; set; } = null!;
    public DateTime RefreshTokenExpireDate { get; set; }
}
