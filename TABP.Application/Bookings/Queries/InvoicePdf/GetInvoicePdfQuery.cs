using MediatR;

namespace TABP.Application.Bookings.Queries.PdfConfirmation;
public class GetInvoicePdfQuery : IRequest<InvoicePdfResponse>
{
    public Guid BookingId { get; set; }
}
