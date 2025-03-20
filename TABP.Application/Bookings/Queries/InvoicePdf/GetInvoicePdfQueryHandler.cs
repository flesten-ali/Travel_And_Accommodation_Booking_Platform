using MediatR;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Interfaces.Services.Html;
using TABP.Domain.Interfaces.Services.Pdf;

namespace TABP.Application.Bookings.Queries.InvoicePdf;

/// <summary>
/// Handles the query for retrieving an invoice PDF for a booking.
/// </summary>
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

    /// <summary>
    /// Handles the request to generate an invoice PDF for a specific booking.
    /// </summary>
    /// <param name="request">The query containing the booking ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The <see cref="InvoicePdfResponse"/> containing the PDF file for the invoice.</returns>
    /// <exception cref="NotFoundException">Thrown if the booking is not found.</exception>
    public async Task<InvoicePdfResponse> Handle(GetInvoicePdfQuery request, CancellationToken cancellationToken = default)
    {
        var booking = await _bookingRepository.GetByIdIncludePropertiesAsync(
            request.BookingId,
            cancellationToken,
            b => b.Invoice)
            ?? throw new NotFoundException(BookingExceptionMessages.NotFound);

        var invoiceHtml = _invoiceHtmlGenerationService.GenerateHtml(booking);
        var invoicePdf = await _pdfService.GeneratePdfAsync(invoiceHtml, cancellationToken);

        return new InvoicePdfResponse(invoicePdf);
    }
}
