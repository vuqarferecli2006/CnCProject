using Microsoft.AspNetCore.Http;

namespace CnC.Application.Shared.Helpers.ContextHelper;

public static class HttpContextExtensions
{
    public static string? ResolveSessionId(this HttpContext? ctx)
    {
        if (ctx == null) 
            return null;
        
        //Check request header
        var header = ctx.Request.Headers["X-Session-Id"].FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(header)) 
            return header;

        //Check query string
        var query = ctx.Request.Query["sessionId"].FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(query)) 
            return query;

        //Fallback to ASP.NET Core session Id
        return ctx.Session?.Id;
    }
}
