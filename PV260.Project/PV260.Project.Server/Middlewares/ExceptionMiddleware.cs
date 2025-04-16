using System.Net;

namespace PV260.Project.Server.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        (HttpStatusCode statusCode, string message) = exception switch
        {
            // Add custom exceptions to convert to proper HTTP status code
            _ => (HttpStatusCode.InternalServerError, "<REPLACE_WITH_MESSAGE_FROM_CONSTANTS>")
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            StatusCode = (int)statusCode,
            Message = message
        };

        await context.Response.WriteAsJsonAsync(response);
    }
}
