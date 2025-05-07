using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using TABP.WebAPI.Common;
using TABP.WebAPI.Filters;
using TABP.WebAPI.Middlewares;

namespace TABP.WebAPI;

public static class WebApiDependencyInjection
{
    public static IServiceCollection AddWebApi(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddControllers()
                       .AddProblemDetails()
                       .AddExceptionHandler<GlobalExceptionHandler>()
                       .AddRateLimiter(configuration);
    }

    private static IServiceCollection AddControllers(this IServiceCollection services)
    {
        var presentationAssembly = typeof(Presentation.AssemblyReference).Assembly;

        services.AddControllers(opt =>
        {
            opt.Filters.Add<LoggingFilter>();
        })
       .AddJsonOptions(opt =>
       {
           opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
           opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
       })
       .AddApplicationPart(presentationAssembly);

        return services;
    }

    private static IServiceCollection AddRateLimiter(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<RateLimiterConfig>()
           .Bind(configuration.GetSection(nameof(RateLimiterConfig)));
        
        var config = configuration.GetSection(nameof(RateLimiterConfig))
            .Get<RateLimiterConfig>();

        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = 429;
            options.AddTokenBucketLimiter(
                     "Rate limiter",
                    options =>
                    {
                        options.TokenLimit = config.TokenLimit;
                        options.TokensPerPeriod = config.TokensPerPeriod;
                        options.QueueLimit = config.QueueLimit;
                        options.QueueProcessingOrder = QueueProcessingOrder.NewestFirst;
                        options.AutoReplenishment = config.AutoReplenishment;
                        options.ReplenishmentPeriod = TimeSpan.FromSeconds(config.ReplenishmentPeriod);
                    });
        });

        return services;
    }
}
