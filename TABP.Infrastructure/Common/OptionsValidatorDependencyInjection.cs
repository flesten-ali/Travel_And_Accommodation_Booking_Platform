using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TABP.Infrastructure.Security.Jwt;

namespace TABP.Infrastructure.Common;
public static class OptionsValidatorDependencyInjection
{
    public static IServiceCollection AddOptionsValidator(this IServiceCollection services)
    {
        return services.AddValidatorsFromAssemblyContaining<JwtOptionsValidator>(ServiceLifetime.Singleton);
    }
}
