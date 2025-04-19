using AutoMapper;
using MediatR;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Hotels.Queries.GetFeaturedDeals;

/// <summary>
/// Handles the query to retrieve a list of featured hotel deals.
/// </summary>
public class GetFeaturedDealsQueryHandler : IRequestHandler<GetFeaturedDealsQuery, IEnumerable<FeaturedDealResponse>>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IMapper _mapper;

    public GetFeaturedDealsQueryHandler(IHotelRepository hotelRepository, IMapper mapper)
    {
        _hotelRepository = hotelRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the query to retrieve a list of featured hotel deals.
    /// </summary>
    /// <param name="request">The query containing the limit for the number of featured deals to retrieve.</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the operation if needed.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The result is a collection of <see cref="FeaturedDealResponse"/>.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown if the limit specified in the query is less than 1.</exception>
    public async Task<IEnumerable<FeaturedDealResponse>> Handle(
        GetFeaturedDealsQuery request,
        CancellationToken cancellationToken = default)
    {
        if (request.Limit < 1)
            throw new ArgumentException(ValidationExceptionMessages.LimitGreaterThanZero);

        var featuredDeals = await _hotelRepository.GetFeaturedDealsAsync(
            request.Limit,
            cancellationToken);

        return _mapper.Map<IEnumerable<FeaturedDealResponse>>(featuredDeals);
    }
}