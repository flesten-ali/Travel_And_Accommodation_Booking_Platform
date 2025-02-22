using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TABP.Presentation.Validators.User;
namespace TABP.Presentation;

public static class PresentationDependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddMapper()
                .AddFluentValidations();

        return services;
    }

    private static IServiceCollection AddMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        return services;
    }

    public static IServiceCollection AddFluentValidations(this IServiceCollection services)
    {
        return services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<RegisterUserRequestValidator>());
    }
}
