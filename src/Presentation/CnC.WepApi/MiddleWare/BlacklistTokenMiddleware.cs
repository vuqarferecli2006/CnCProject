using CnC.Application.Abstracts.Services;

namespace CnC.WepApi.MiddleWare;

public class BlacklistTokenMiddleware
{
    private readonly RequestDelegate _next;

    public BlacklistTokenMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var tokenBlacklistService = context.RequestServices.GetRequiredService<ITokenBlacklistService>();

        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
        {
            var token = authHeader.Substring("Bearer ".Length).Trim();

            if (await tokenBlacklistService.IsTokenBlacklistedAsync(token))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
        }

        await _next(context);
    }
}
