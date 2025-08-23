namespace CnC.Application.Abstracts.Services;

public interface ITokenBlacklistService
{
    Task BlacklistTokenAsync(string token, DateTime expiry);
    Task<bool> IsTokenBlacklistedAsync(string token);
}
