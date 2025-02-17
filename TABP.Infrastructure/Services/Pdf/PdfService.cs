using NReco.PdfGenerator;
using TABP.Domain.Interfaces.Services.Pdf;
namespace TABP.Infrastructure.Services.Pdf;

public class PdfService : IPdfService
{
    public Task<byte[]> GeneratePdfAsync(string html, CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            cancellationToken.ThrowIfCancellationRequested();

            var htmlToPdfConverter = new HtmlToPdfConverter();
            return htmlToPdfConverter.GeneratePdf(html);

        }, cancellationToken);
    }
}