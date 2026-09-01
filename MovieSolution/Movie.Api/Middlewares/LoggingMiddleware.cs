using System.Diagnostics;

namespace Movie.Api.Middlewares;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        _logger.LogInformation("{Method} {Path}", context.Request.Method, context.Request.Path);
        Stopwatch sw = Stopwatch.StartNew();
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(
                "{Method} {Path} threw an error:\n{Message}",
                context.Request.Method,
                context.Request.Path,
                e.Message
            );
            throw;
        }
        finally
        {
            sw.Stop();
        }
        if (context.Response.StatusCode >= 400)
        {
            _logger.LogWarning(
                "{Method} {Path} has failed with statuscode: {StatusCode}",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode
            );
        }
        else
        {
            _logger.LogInformation(
                "{Method} {Path} took {ElapsedMilliseconds}ms",
                context.Request.Method,
                context.Request.Path,
                sw.ElapsedMilliseconds
            );
        }
    }
}
