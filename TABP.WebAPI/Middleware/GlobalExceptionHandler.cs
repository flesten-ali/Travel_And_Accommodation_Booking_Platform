using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using TABP.Domain.Exceptions;

namespace TABP.WebAPI.Middleware;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> _logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(
            exception,
            "Exception occurred  while processing request {Method} {Path} with message {Message}",
            httpContext.Request.Method,
            httpContext.Request.Path,
            exception.Message);

        var problemDetails = CreateProblemDetails(httpContext, exception);

        httpContext.Response.StatusCode = problemDetails.Status!.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private static ProblemDetails CreateProblemDetails(HttpContext httpContext, Exception exception)
    {
        int statusCode = exception switch
        {
            NotFoundException => (int)HttpStatusCode.NotFound,
            ConflictException => (int)HttpStatusCode.Conflict,
            UnauthorizedException => (int)HttpStatusCode.Unauthorized,
            BadRequestException => (int)HttpStatusCode.BadRequest,
            _ => (int)HttpStatusCode.InternalServerError,
        };

        return new ProblemDetails
        {
            Title = exception is DomainException domainException ? domainException.Title : "An Unexpected Error Occurred",
            Detail = exception.Message,
            Status = statusCode,
            Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
            Type = exception.GetType().Name,
            Extensions = new Dictionary<string, object?>
            {
                ["CurrentId"] = Activity.Current?.Id ?? httpContext.TraceIdentifier,
                ["TraceId"] = Convert.ToString(Activity.Current!.Context.TraceId!),
            }
        };
    }
}

