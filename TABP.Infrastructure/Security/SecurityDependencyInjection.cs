using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TABP.Domain.Interfaces.Security.Password;
using TABP.Infrastructure.Security.Jwt;
using TABP.Infrastructure.Security.Password;

namespace TABP.Infrastructure.Security;

public static class SecurityDependencyInjection
{
    public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddJwtAuthentication(configuration)
                  .AddSingleton<IPasswordHasher, PasswordHasher>();
    }
}
