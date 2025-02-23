using AutoMapper;
using MediatR;
using TABP.Application.Bookings.Common;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Bookings.Queries.GetById;
public class GetBookingByIdQueryHandler : IRequestHandler<GetBookingByIdQuery, BookingResponse>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IMapper _mapper;

    public GetBookingByIdQueryHandler(IBookingRepository bookingRepository, IMapper mapper)
    {
        _bookingRepository = bookingRepository;
        _mapper = mapper;
    }

    public async Task<BookingResponse> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken = default)
    {
        var booking = await _bookingRepository.GetByIdIncludePropertiesAsync(
            request.BookingId,
            cancellationToken,
            b => b.Invoice,
            b => b.Rooms) ?? throw new NotFoundException(BookingExceptionMessages.NotFound);

        return _mapper.Map<BookingResponse>(booking);
    }
}
