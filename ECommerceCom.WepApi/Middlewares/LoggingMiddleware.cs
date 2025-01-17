using Serilog;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;

    public LoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var requestUrl = context.Request.Path;
        var requestTime = DateTime.UtcNow;
        var userId = context.User.Identity.IsAuthenticated ? context.User.Identity.Name : "Anonymous";

        // Serilog ile log yazma
        Log.Information($"[{requestTime}] - User: {userId} - Request URL: {requestUrl}");

        await _next(context);
    }
}
