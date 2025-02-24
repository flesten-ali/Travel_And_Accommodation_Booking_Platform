using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;
using TABP.WebAPI.Filter;
using TABP.WebAPI.Middleware;

namespace TABP.WebAPI;

public static class WebApiDependencyInjection
{
    public static IServiceCollection AddWebApi(this IServiceCollection services)
    {
        return services.AddControllers()
                       .AddProblemDetails()
                       .AddExceptionHandler<GlobalExceptionHandler>()
                       .AddApiVersioning()
                       .AddSwagger();

    }

    public static IServiceCollection AddControllers(this IServiceCollection services)
    {
        var presentaionAssembly = typeof(Presentation.AssemblyReference).Assembly;

        services.AddControllers(opt =>
        {
            opt.Filters.Add<LoggingFilter>();
        })
       .AddJsonOptions(opt =>
       {
           opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
           opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
       })
       .AddApplicationPart(presentaionAssembly);

        return services;
    }

    public static IServiceCollection AddApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(opt =>
        {
            opt.ReportApiVersions = true;
            opt.AssumeDefaultVersionWhenUnspecified = true;
            opt.DefaultApiVersion = new ApiVersion(1, 0);
        })
        .AddMvc()
        .AddApiExplorer(opt =>
        {
            opt.SubstituteApiVersionInUrl = true;
        });

        return services;
    }

    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        var apiVersionDescriptionProvider = services.BuildServiceProvider()
                .GetRequiredService<IApiVersionDescriptionProvider>();

        services.AddSwaggerGen(setupAction =>
        {
            foreach (var des in apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                setupAction.SwaggerDoc(
                    $"{des.GroupName}", new()
                    {
                        Title = "Travel and Accommodation Booking Platform",
                        Version = des.ApiVersion.ToString(),
                    });
            }

            setupAction.AddSecurityDefinition("Authentication", new()
            {
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                Description = "Input a valid token to be authenticated",
            });

            setupAction.AddSecurityRequirement(new()
            {
                {
                    new()
                    {
                        Reference = new OpenApiReference()
                        {
                            Type= ReferenceType.SecurityScheme,
                            Id = "Authentication"
                        },
                    },
                    new List<string>()
                }
            });

            var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
            setupAction.IncludeXmlComments(xmlCommentsFullPath);
        });
        return services;
    }
}
