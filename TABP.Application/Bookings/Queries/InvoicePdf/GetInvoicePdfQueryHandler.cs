using AutoMapper;
using MediatR;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Interfaces.Services.Html;
using TABP.Domain.Interfaces.Services.Pdf;

namespace TABP.Application.Bookings.Queries.PdfConfirmation;
public class GetInvoicePdfQueryHandler : IRequestHandler<GetInvoicePdfQuery, InvoicePdfResponse>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IInvoiceHtmlGenerationService _invoiceHtmlGenerationService;
    private readonly IPdfService _pdfService;
    private readonly IMapper _mapper;

    public GetInvoicePdfQueryHandler(IBookingRepository bookingRepository,
        IInvoiceHtmlGenerationService invoiceHtmlGenerationService,
        IPdfService pdfService,
        IMapper mapper)
    {
        _bookingRepository = bookingRepository;
        _invoiceHtmlGenerationService = invoiceHtmlGenerationService;
        _pdfService = pdfService;
        _mapper = mapper;
    }

    public async Task<InvoicePdfResponse> Handle(GetInvoicePdfQuery request, CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.GetByIdAsync(request.BookingId)
            ?? throw new NotFoundException("Booking not found");

        var invoiceHtml = _invoiceHtmlGenerationService.GenerateHtml(booking);
        var invoicePdf = await _pdfService.GeneratePdfAsync(invoiceHtml);

        return new InvoicePdfResponse
        {
            PdfContent = invoicePdf,
        };
    }
}
