using AutoMapper;
using MediatR;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Cities.Queries.GetTrending;

/// <summary>
/// Handles the query to retrieve a list of trending cities based on bookings.
/// </summary>
public class GetTrendingCitiesQueryHandler
    : IRequestHandler<GetTrendingCitiesQuery, IEnumerable<TrendingCitiesResponse>>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IMapper _mapper;

    public GetTrendingCitiesQueryHandler(IBookingRepository bookingRepository, IMapper mapper)
    {
        _bookingRepository = bookingRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the request to retrieve a list of trending cities based on bookings, limited by the specified number.
    /// </summary>
    /// <param name="request">The request containing the limit for the number of trending cities.</param>
    /// <param name="cancellationToken">The cancellation token for handling task cancellation.</param>
    /// <returns>
    /// A task that represents the asynchronous operation, containing a list of trending cities mapped to <see cref="TrendingCitiesResponse"/>.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when the <paramref name="request.Limit"/> is less than 1.</exception>
    public async Task<IEnumerable<TrendingCitiesResponse>> Handle(
        GetTrendingCitiesQuery request,
        CancellationToken cancellationToken = default)
    {
        if (request.Limit < 1)
            throw new ArgumentException(ValidationExceptionMessages.LimitGreaterThanZero);

        var trendingHotels = await _bookingRepository.GetTrendingCitiesAsync(request.Limit, cancellationToken);

        return _mapper.Map<IEnumerable<TrendingCitiesResponse>>(trendingHotels);
    }
}
