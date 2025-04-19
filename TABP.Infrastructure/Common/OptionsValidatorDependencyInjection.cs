using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace TABP.Infrastructure.Common;

public static class OptionsValidatorDependencyInjection
{
    public static IServiceCollection AddOptionsValidator(this IServiceCollection services)
    {
        return services.AddValidatorsFromAssemblyContaining<AssemblyReference>(ServiceLifetime.Singleton);
    }
}
