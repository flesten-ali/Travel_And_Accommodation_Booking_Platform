using MediatR;

namespace TABP.Application.Bookings.Queries.PdfConfirmation;
public class GetInvoicePdfQuery : IRequest<GetInvoicePdfResponse>
{
    public Guid BookingId { get; set; }
}
