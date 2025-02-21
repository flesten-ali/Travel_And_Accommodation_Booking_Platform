using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TABP.Domain.Interfaces.Security.Jwt;
using TABP.Infrastructure.Common;

namespace TABP.Infrastructure.Security.Jwt;

public static class JwtAuthConfiguration
{


    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<JwtConfig>()
            .Bind(configuration.GetSection(nameof(JwtConfig)))
            .ValidateFluently()
            .ValidateOnStart();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            using var scope = services.BuildServiceProvider().CreateScope();

            var config = scope.ServiceProvider.GetRequiredService<IOptions<JwtConfig>>().Value;

            var key = Encoding.UTF8.GetBytes(config.Key);

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = config.Issuer,
                ValidateAudience = true,
                ValidAudience = config.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });
        services.AddTransient<IJwtGenerator, JwtGenerator>();
        return services;
    }
}