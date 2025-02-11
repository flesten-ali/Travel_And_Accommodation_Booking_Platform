using MediatR;
using TABP.Application.Bookings.Common;
namespace TABP.Application.Bookings.Queries.GetBookingById;

public class GetBookingByIdQuery : IRequest<BookingResponse>
{
    public Guid BookingId { get; set; }
}