using MediatR;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Interfaces.Services.Html;
using TABP.Domain.Interfaces.Services.Pdf;
namespace TABP.Application.Bookings.Queries.InvoicePdf;

public class GetInvoicePdfQueryHandler : IRequestHandler<GetInvoicePdfQuery, InvoicePdfResponse>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IInvoiceHtmlGenerationService _invoiceHtmlGenerationService;
    private readonly IPdfService _pdfService;

    public GetInvoicePdfQueryHandler(IBookingRepository bookingRepository,
        IInvoiceHtmlGenerationService invoiceHtmlGenerationService,
        IPdfService pdfService)
    {
        _bookingRepository = bookingRepository;
        _invoiceHtmlGenerationService = invoiceHtmlGenerationService;
        _pdfService = pdfService;
    }

    public async Task<InvoicePdfResponse> Handle(GetInvoicePdfQuery request, CancellationToken cancellationToken = default)
    {
        var booking = await _bookingRepository.GetByIdIncludePropertiesAsync(
            request.BookingId,
            cancellationToken,
            b => b.Invoice)
            ?? throw new NotFoundException(BookingExceptionMessages.NotFound);

        var invoiceHtml = _invoiceHtmlGenerationService.GenerateHtml(booking);
        var invoicePdf = await _pdfService.GeneratePdfAsync(invoiceHtml, cancellationToken);

        return new InvoicePdfResponse
        {
            PdfContent = invoicePdf,
        };
    }
}
