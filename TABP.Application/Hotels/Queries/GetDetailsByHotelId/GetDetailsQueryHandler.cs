using AutoMapper;
using MediatR;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence.Repositories;
namespace TABP.Application.Hotels.Queries.GetDetailsByHotelId;

public class GetDetailsQueryHandler : IRequestHandler<GetDetailsQuery, GetDetailsResponse>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IMapper _mapper;

    public GetDetailsQueryHandler(IHotelRepository hotelRepository, IMapper mapper)
    {
        _hotelRepository = hotelRepository;
        _mapper = mapper;
    }
    public async Task<GetDetailsResponse> Handle(GetDetailsQuery request, CancellationToken cancellationToken)
    {
        var hotel = await _hotelRepository.GetHotelByIdAsync(request.HotelId, h => h.Gallery.Where(g => g.ImageableId == h.Id));

        if (hotel == null)
        {
            throw new NotFoundException("Hotel not found");
        }

        return _mapper.Map<GetDetailsResponse>(hotel);
    }
}
