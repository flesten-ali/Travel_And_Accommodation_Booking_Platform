using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace TABP.WebAPI.Filter;

public class LoggingFilter(ILogger<LoggingFilter> logger) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var body = JsonSerializer.Serialize(context.ActionArguments.ToDictionary(k => k.Key, v => v.Value.ToString()));

        logger.LogInformation(
            "Incoming Request: {Method} {Path}, Body: {body}",
            context.HttpContext.Request.Method,
            context.HttpContext.Request.Path,
            body);

        await next();

        logger.LogInformation(
            "Response: {Path} returned {StatusCode}",
            context.HttpContext.Request.Path,
            context.HttpContext.Response.StatusCode);
    }
}
