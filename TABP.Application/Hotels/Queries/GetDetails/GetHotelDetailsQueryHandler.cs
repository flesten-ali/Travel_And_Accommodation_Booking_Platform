using AutoMapper;
using MediatR;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence.Repositories;
namespace TABP.Application.Hotels.Queries.GetDetails;

public class GetHotelDetailsQueryHandler : IRequestHandler<GetHotelDetailsQuery, GetHotelDetailsResponse>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IMapper _mapper;

    public GetHotelDetailsQueryHandler(IHotelRepository hotelRepository, IMapper mapper)
    {
        _hotelRepository = hotelRepository;
        _mapper = mapper;
    }
    public async Task<GetHotelDetailsResponse> Handle(GetHotelDetailsQuery request, CancellationToken cancellationToken)
    {
        var hotel = await _hotelRepository.GetByIdIncludeProperties(request.HotelId, h => h.Gallery.Where(g => g.ImageableId == h.Id));

        if (hotel == null)
        {
            throw new NotFoundException("Hotel not found");
        }

        return _mapper.Map<GetHotelDetailsResponse>(hotel);
    }
}
