using AutoMapper;
using MediatR;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Cities.GetTrending;
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

    public async Task<IEnumerable<TrendingCitiesResponse>> Handle(GetTrendingCitiesQuery request, CancellationToken cancellationToken)
    {
        var trendingHotels = await _bookingRepository.GetTrendingCities(request.Limit);

        return _mapper.Map<IEnumerable<TrendingCitiesResponse>>(trendingHotels);
    }
}
