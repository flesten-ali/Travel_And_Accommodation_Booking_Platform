using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace TABP.Presentation;

public static class PresentationDependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddMapper()
                .AddFluentValidations()
                .AddApiVersioning()
                .AddSwagger();

        return services;
    }

    private static IServiceCollection AddApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(opt =>
        {
            opt.ReportApiVersions = true;
            opt.AssumeDefaultVersionWhenUnspecified = true;
            opt.DefaultApiVersion = new ApiVersion(1, 0);
            opt.ApiVersionReader = new UrlSegmentApiVersionReader();
        })
        .AddMvc()
        .AddApiExplorer(opt =>
        {
            opt.GroupNameFormat = "'v'V";
            opt.SubstituteApiVersionInUrl = true;
        });

        return services;
    }

    private static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        var apiVersionDescriptionProvider = services.BuildServiceProvider()
                .GetRequiredService<IApiVersionDescriptionProvider>();

        services.AddSwaggerGen(setupAction =>
        {
            foreach (var desc in apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                setupAction.SwaggerDoc(
                    $"{desc.GroupName}", new()
                    {
                        Title = "Travel and Accommodation Booking Platform",
                        Version = desc.ApiVersion.ToString(),
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

    private static IServiceCollection AddMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        return services;
    }

    private static IServiceCollection AddFluentValidations(this IServiceCollection services)
    {
        return services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<AssemblyReference>());
    }
}
