using MediatR;
namespace TABP.Application.Bookings.Queries.InvoicePdf;

public class GetInvoicePdfQuery : IRequest<InvoicePdfResponse>
{
    public Guid BookingId { get; set; }
}
