using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TABP.Domain.Interfaces.Auth;

namespace TABP.Infrastructure.Auth.Jwt;
public static class JwtAuthConfiguration
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtConfig>(configuration.GetSection(nameof(JwtConfig)));
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            using var scope = services.BuildServiceProvider().CreateScope();

            var config = scope.ServiceProvider
              .GetRequiredService<IOptions<JwtConfig>>().Value;

            var key = Encoding.UTF8.GetBytes(config.Key);

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = config.Issuer,
                ValidAudience = config.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero
            };
        });
        services.AddTransient<IJwtGenerator, JwtGenerator>();
        return services;
    }
}
