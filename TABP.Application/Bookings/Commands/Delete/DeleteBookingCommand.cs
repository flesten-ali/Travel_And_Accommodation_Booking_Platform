using MediatR;

namespace TABP.Application.Bookings.Commands.Delete;
public class DeleteBookingCommand :IRequest
{
    public Guid Id { get; set; }
}
