using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace TABP.WebAPI.Filters;

/// <summary>
/// An action filter that logs incoming HTTP requests and outgoing responses.
/// </summary>
/// <remarks>
/// This filter serializes the action arguments for logging and then logs
/// the HTTP method, request path, and response status code.
/// </remarks>
/// <param name="logger">The logger instance used to log the request and response details.</param>
public class LoggingFilter(ILogger<LoggingFilter> logger) : IAsyncActionFilter
{
    /// <summary>
    /// Called asynchronously before and after the action executes.
    /// Logs the request details before the action is executed and logs the response details after the action has executed.
    /// </summary>
    /// <param name="context">
    /// The <see cref="ActionExecutingContext"/> which contains the HTTP context and action parameters.
    /// </param>
    /// <param name="next">
    /// The <see cref="ActionExecutionDelegate"/> used to execute the action.
    /// </param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var body = JsonSerializer.Serialize(
            context.ActionArguments.ToDictionary(
                k => k.Key,
                v => v.Value?.ToString()
            ));

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