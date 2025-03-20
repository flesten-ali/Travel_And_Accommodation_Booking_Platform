﻿using System.Text.Json.Serialization;
using TABP.WebAPI.Filters;
using TABP.WebAPI.Middlewares;

namespace TABP.WebAPI;

public static class WebApiDependencyInjection
{
    public static IServiceCollection AddWebApi(this IServiceCollection services)
    {
        return services.AddControllers()
                       .AddProblemDetails()
                       .AddExceptionHandler<GlobalExceptionHandler>();
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
}
