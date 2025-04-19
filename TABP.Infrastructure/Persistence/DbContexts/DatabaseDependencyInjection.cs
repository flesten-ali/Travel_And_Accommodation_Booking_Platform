using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TABP.Domain.Interfaces.Persistence;

namespace TABP.Infrastructure.Persistence.DbContexts;

public static class DatabaseDependencyInjection
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("SqlServer"),
                sqlOptions => sqlOptions.EnableRetryOnFailure(5));
        });
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    public static async Task ApplyMigrationAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (context.Database.GetPendingMigrations().Any())
        {
            await context.Database.MigrateAsync();
        }
    }
}
