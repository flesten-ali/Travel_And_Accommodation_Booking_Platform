using MediatR;
using Microsoft.Extensions.Logging;

namespace TABP.Application.Behaviors;
public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

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
