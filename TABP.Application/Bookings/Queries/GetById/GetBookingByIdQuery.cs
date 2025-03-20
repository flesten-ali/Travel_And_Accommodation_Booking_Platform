using MediatR;
using TABP.Application.Bookings.Common;
namespace TABP.Application.Bookings.Queries.GetById;

public sealed record GetBookingByIdQuery(Guid BookingId) : IRequest<BookingResponse>;
