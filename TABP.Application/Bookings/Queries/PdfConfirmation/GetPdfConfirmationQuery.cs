using MediatR;

namespace TABP.Application.Bookings.Queries.PdfConfirmation;
public class GetPdfConfirmationQuery : IRequest<GetPdfConfirmationResponse>
{
    public Guid BookingId { get; set; }
}
