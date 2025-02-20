using MediatR;

namespace TABP.Application.Bookings.Commands.Delete;
public class DeleteBookingCommand :IRequest
{
    public Guid BookingId { get; set; }
    public Guid UserId { get; set; }
}
