using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using TABP.Domain.Exceptions;

namespace TABP.WebAPI.Middlewares;

/// <summary>
/// Global exception handler middleware that intercepts unhandled exceptions, logs them,
/// and writes a <see cref="ProblemDetails"/> response to the HTTP response.
/// </summary>
public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    /// <summary>
    /// Handles exceptions that occur during request processing.
    /// Logs the exception details and writes a standardized error response.
    /// </summary>
    /// <param name="httpContext">The current HTTP context.</param>
    /// <param name="exception">The exception that occurred.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>
    /// A <see cref="ValueTask{TResult}"/> that resolves to <c>true</c> indicating the exception has been handled.
    /// </returns>
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(
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

    /// <summary>
    /// Creates a <see cref="ProblemDetails"/> object based on the provided exception and HTTP context.
    /// Maps specific domain exceptions to corresponding HTTP status codes.
    /// </summary>
    /// <param name="httpContext">The HTTP context from which request details are extracted.</param>
    /// <param name="exception">The exception to be transformed into a <see cref="ProblemDetails"/> response.</param>
    /// <returns>A populated <see cref="ProblemDetails"/> instance with error details and HTTP status.</returns>
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

