using AutoMapper;
using MediatR;
using TABP.Application.Bookings.Common;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Bookings.Queries.GetById;

/// <summary>
/// Handles queries for retrieving a booking by its ID.
/// </summary>
public class GetBookingByIdQueryHandler : IRequestHandler<GetBookingByIdQuery, BookingResponse>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IMapper _mapper;

    public GetBookingByIdQueryHandler(IBookingRepository bookingRepository, IMapper mapper)
    {
        _bookingRepository = bookingRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the request to retrieve a booking by ID.
    /// </summary>
    /// <param name="request">The query containing the booking ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The <see cref="BookingResponse"/> representing the retrieved booking.</returns>
    /// <exception cref="NotFoundException">Thrown if the booking is not found.</exception>
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
