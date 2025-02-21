using TABP.WebAPI.Middleware;

namespace TABP.WebAPI;

public static class WebApiDependencyInjection
{
    public static IServiceCollection AddWebApi(this IServiceCollection services)
    {
        return services.AddProblemDetails()
                       .AddExceptionHandler<GlobalExceptionHandler>();
    }
}
