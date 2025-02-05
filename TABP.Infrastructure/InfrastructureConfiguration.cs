using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TABP.Domain.Interfaces.Auth;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Interfaces.Services.Email;
using TABP.Domain.Interfaces.Services.Html;
using TABP.Domain.Interfaces.Services.Image;
using TABP.Domain.Interfaces.Services.Pdf;
using TABP.Domain.Models;
using TABP.Infrastructure.Auth;
using TABP.Infrastructure.Auth.Jwt;
using TABP.Infrastructure.Persistence.DbContexts;
using TABP.Infrastructure.Persistence.Repositories;
using TABP.Infrastructure.Services.Email;
using TABP.Infrastructure.Services.Html;
using TABP.Infrastructure.Services.Image;
using TABP.Infrastructure.Services.Pdf;
namespace TABP.Infrastructure;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddDatabase(configuration)
                       .AddRespositories()
                       .AddServices(configuration)
                       .AddJwtAuthentication(configuration);
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
          options.UseSqlServer(configuration.GetConnectionString("SqlServer"))
        );
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    public static IServiceCollection AddRespositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IOwnerRepository, OwnerRepository>();
        services.AddScoped<IHotelRepository, HotelRepository>();
        services.AddScoped<IAmenityRepository, AmenityRepository>();
        services.AddScoped<IRoomClassRepository, RoomClassRepository>();
        services.AddScoped<IImageRepository, ImageRepository>();
        services.AddScoped<ICityRepository, CityRepository>();
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IPasswordHasher, PasswordHasher>();

        services.AddOptions<StorageFolderConfig>()
                 .Bind(configuration.GetSection(nameof(StorageFolderConfig)));

        services.AddSingleton<IImageStorageService>(sp =>
        {
            var storageFolderConfig = sp.GetRequiredService<IOptions<StorageFolderConfig>>().Value;
            var folderPath = Directory.GetCurrentDirectory() + storageFolderConfig.Name;
            return new ImageStorageService(folderPath);
        });

        services.AddSingleton<IPdfService, PdfService>();

        services.AddSingleton<IInvoiceHtmlGenerationService, InvoiceHtmlGenerationService>();

        services.AddOptions<SMTPConfig>()
                .Bind(configuration.GetSection(nameof(SMTPConfig)));
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<SMTPConfig>>().Value);

        services.AddSingleton<IEmailSenderService, EmailSenderService>();
        return services;
    }
}