using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;
using TABP.Presentation.Controllers;
using TABP.Presentation.Validators.User;
namespace TABP.Presentation;

public static class PresentationConfiguration
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        var presentaionAssembly = typeof(UsersController).Assembly;
        services.AddSwagger()
                .AddMapper()
                .AddFluentValidations()
                .AddControllers()
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                })
                .AddApplicationPart(presentaionAssembly);

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

    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(setupAction =>
        {
            setupAction.EnableAnnotations();

            setupAction.AddSecurityDefinition("Authentication", new()
            {
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                Description = "Input a valid Token to be authenticated"
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
        });
        return services;
    }
}
