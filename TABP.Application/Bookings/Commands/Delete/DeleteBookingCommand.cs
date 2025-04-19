using MediatR;

namespace TABP.Application.Bookings.Commands.Delete;
public sealed record DeleteBookingCommand(Guid BookingId, Guid UserId) : IRequest;
