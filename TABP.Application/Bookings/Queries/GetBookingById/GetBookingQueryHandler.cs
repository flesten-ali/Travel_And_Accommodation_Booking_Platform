using AutoMapper;
using MediatR;
using TABP.Application.Bookings.Common;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Bookings.Queries.GetBookingById;
public class GetBookingQueryHandler : IRequestHandler<GetBookingQuery, BookingResponse>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IMapper _mapper;

    public GetBookingQueryHandler(IBookingRepository bookingRepository, IMapper mapper)
    {
        _bookingRepository = bookingRepository;
        _mapper = mapper;
    }

    public async Task<BookingResponse> Handle(GetBookingQuery request, CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.GetByIdIncludeProperties(
            request.BookingId,
            b => b.Invoice,
            b => b.Rooms) ?? throw new NotFoundException("Booking not found");

        return _mapper.Map<BookingResponse>(booking);
    }
}
