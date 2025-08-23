using CnC.Application.Abstracts.Services;
using Microsoft.Extensions.Caching.Memory;

namespace CnC.Infrastructure.Services;

public class InMemoryTokenBlacklistService : ITokenBlacklistService
{
    private readonly IMemoryCache _cache;
    private const string BlacklistPrefix = "blacklist_";

    public InMemoryTokenBlacklistService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public Task BlacklistTokenAsync(string token, DateTime expiry)
    {
        var expireDate=expiry - DateTime.UtcNow;

        if(expireDate <= TimeSpan.Zero)
            return Task.CompletedTask;

        _cache.Set(BlacklistPrefix + token, "blacklisted", expireDate);

        return Task.CompletedTask;

    }

    public Task<bool> IsTokenBlacklistedAsync(string token)
    {
        var exists = _cache.TryGetValue(BlacklistPrefix + token, out _);
        return Task.FromResult(exists);
    }
}
