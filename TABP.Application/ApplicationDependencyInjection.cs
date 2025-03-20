using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TABP.Application.Behaviors;
namespace TABP.Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services.AddMediator()
                       .AddMapper()
                       .AddPipelineBehavior();
    }

    private static IServiceCollection AddMediator(this IServiceCollection services)
    {
        return services.AddMediatR(Assembly.GetExecutingAssembly());
    }

    private static IServiceCollection AddMapper(this IServiceCollection services)
    {
        return services.AddAutoMapper(Assembly.GetExecutingAssembly());
    }

    private static IServiceCollection AddPipelineBehavior(this IServiceCollection services)
    {
        return services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
    }
}