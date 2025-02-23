using MediatR;
namespace TABP.Application.Bookings.Queries.InvoicePdf;

public sealed record class GetInvoicePdfQuery(Guid BookingId) : IRequest<InvoicePdfResponse>;
 