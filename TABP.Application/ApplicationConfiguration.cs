using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
namespace TABP.Application;

public static class ApplicationConfiguration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services.AddMediator()
                       .AddMapper();
    }
  
    public static IServiceCollection AddMediator(this IServiceCollection services)
    {
        return services.AddMediatR(Assembly.GetExecutingAssembly());
    }

    public static IServiceCollection AddMapper(this IServiceCollection services)
    {
        return services.AddAutoMapper(Assembly.GetExecutingAssembly());
    }
}