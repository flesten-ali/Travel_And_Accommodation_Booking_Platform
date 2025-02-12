using AutoMapper;
using MediatR;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Hotels.Queries.GetFeaturedDeals;
public class GetFeaturedDealsQueryHandler : IRequestHandler<GetFeaturedDealsQuery, IEnumerable<FeaturedDealResponse>>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IMapper _mapper;

    public GetFeaturedDealsQueryHandler(IHotelRepository hotelRepository, IMapper mapper)
    {
        _hotelRepository = hotelRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<FeaturedDealResponse>> Handle(GetFeaturedDealsQuery request, CancellationToken cancellationToken)
    {
        if (request.Limit < 1)
            throw new ArgumentException("The number of deals must be greater than zero");

        var featuredDeals = await _hotelRepository.GetFeaturedDeals(request.Limit);

        return _mapper.Map<IEnumerable<FeaturedDealResponse>>(featuredDeals);
    }
}
