namespace CnC.Application.Shared.Settings;

public class JwtSetting
{
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public int ExpiresInMinutes { get; set; }
}
