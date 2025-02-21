using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TABP.Domain.Interfaces.Services.Date;
using TABP.Domain.Interfaces.Services.Email;
using TABP.Domain.Interfaces.Services.Guids;
using TABP.Domain.Interfaces.Services.Html;
using TABP.Domain.Interfaces.Services.Image;
using TABP.Domain.Interfaces.Services.Pdf;
using TABP.Infrastructure.Common;
using TABP.Infrastructure.Services.Date;
using TABP.Infrastructure.Services.Email;
using TABP.Infrastructure.Services.Guids;
using TABP.Infrastructure.Services.Html;
using TABP.Infrastructure.Services.Image;
using TABP.Infrastructure.Services.Pdf;

namespace TABP.Infrastructure.Services;
public static class ServicesDependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IPdfService, PdfService>();

        services.AddSingleton<IInvoiceHtmlGenerationService, InvoiceHtmlGenerationService>();

        services.AddOptions<SMTPConfig>()
                .Bind(configuration.GetSection(nameof(SMTPConfig)))
                .ValidateFluently()
                .ValidateOnStart();
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<SMTPConfig>>().Value);

        services.AddSingleton<IEmailSenderService, EmailSenderService>();

        services.AddOptions<CloudinaryConfig>()
                 .Bind(configuration.GetSection(nameof(CloudinaryConfig)))
                 .ValidateFluently()
                 .ValidateOnStart();
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<CloudinaryConfig>>().Value);

        services.AddSingleton<IImageUploadService, CloudinaryImageUploadService>();

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddSingleton<IGuidProvider, GuidProvider>();
        return services;
    }
}
