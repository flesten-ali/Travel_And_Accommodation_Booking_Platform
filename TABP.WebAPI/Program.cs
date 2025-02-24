using Serilog;
using TABP.Application;
using TABP.Infrastructure;
using TABP.Presentation;
using TABP.WebAPI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApplication()
                .AddInfrastructure(builder.Configuration)
                .AddPresentation()
                .AddWebApi();

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

app.UseExceptionHandler();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opt =>
    {
        var descriptions = app.DescribeApiVersions();
        foreach (var desc in descriptions)
        {
            opt.SwaggerEndpoint(
               $"/swagger/{desc.GroupName}/swagger.json",
                 desc.GroupName.ToUpperInvariant()
            );
        }
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
