using MediatR;
using Microsoft.Extensions.Logging;

namespace TABP.Application.Behaviors;

/// <summary>
/// Logging behavior for MediatR pipeline that logs request processing.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Handles the logging behavior before and after processing a request.
    /// </summary>
    /// <param name="request">The request being processed.</param>
    /// <param name="next">The delegate representing the next action in the pipeline.</param>
    /// <param name="cancellationToken">A cancellation token for the operation.</param>
    /// <returns>The response from the next handler in the pipeline.</returns>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Processing request of type {typeof(TRequest).Name}");

        var response = await next();

        _logger.LogInformation($"Completed handling request, response type: {typeof(TResponse).Name}");

        return response;
    }
}
