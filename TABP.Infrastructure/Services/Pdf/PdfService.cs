using NReco.PdfGenerator;
using TABP.Domain.Interfaces.Services.Pdf;

namespace TABP.Infrastructure.Services.Pdf;

public class PdfService : IPdfService
{
    /// <summary>
    /// Generates a PDF document from the provided HTML content asynchronously.
    /// </summary>
    /// <param name="html">The HTML content to convert into a PDF document.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>A task that represents the asynchronous operation and contains the generated PDF as a byte array.</returns>
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