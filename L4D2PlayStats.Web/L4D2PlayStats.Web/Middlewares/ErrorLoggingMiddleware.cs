using Serilog;

namespace L4D2PlayStats.Web.Middlewares;

public class ErrorLoggingMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);

            if (context.Response.StatusCode >= 400)
            {
                Log.Warning("Request resulted in error: {Method} {Path} => {StatusCode}",
                    context.Request.Method,
                    context.Request.Path,
                    context.Response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Unhandled exception during request: {Method} {Path}",
                context.Request.Method,
                context.Request.Path);
            throw;
        }
    }
}