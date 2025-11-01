using FluentResults;

namespace webapi.Controllers.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _request;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    public ErrorHandlingMiddleware(RequestDelegate requestDelegate, ILogger<ErrorHandlingMiddleware> logger)
    {
        _request = requestDelegate;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var response = context.Response;
        try
        {
            await _request(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            response.StatusCode = 500;
            var responseModel = Result.Fail(new Error("Sth wrong with process.").CausedBy(ex));
            await response.WriteAsJsonAsync(responseModel);
        }
    }
}

public static class ErrorHandlingMiddlewareExtension
{
    public static IApplicationBuilder UserErrorLogging(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ErrorHandlingMiddleware>();
    }
}