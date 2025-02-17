using AutoMapper;
using MediatR;
using TABP.Application.Exceptions.Messages;
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

    public async Task<IEnumerable<FeaturedDealResponse>> Handle(
        GetFeaturedDealsQuery request,
        CancellationToken cancellationToken = default)
    {
        if (request.Limit < 1)
            throw new ArgumentException(ValidationExceptionMessages.LimitGreaterThanZero);

        var featuredDeals = await _hotelRepository.GetFeaturedDealsAsync(request.Limit, cancellationToken);

        return _mapper.Map<IEnumerable<FeaturedDealResponse>>(featuredDeals);
    }
}
