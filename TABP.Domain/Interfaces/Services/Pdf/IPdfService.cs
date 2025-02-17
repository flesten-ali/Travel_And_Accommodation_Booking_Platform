namespace TABP.Domain.Interfaces.Services.Pdf;
public interface IPdfService
{
    Task<byte[]> GeneratePdfAsync(string html, CancellationToken cancellationToken = default);
}