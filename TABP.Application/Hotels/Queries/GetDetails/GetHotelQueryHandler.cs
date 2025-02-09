using AutoMapper;
using MediatR;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence.Repositories;
namespace TABP.Application.Hotels.Queries.GetDetails;

public class GetHotelQueryHandler : IRequestHandler<GetHotelQuery, GetHotelResponse>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IMapper _mapper;

    public GetHotelQueryHandler(IHotelRepository hotelRepository, IMapper mapper)
    {
        _hotelRepository = hotelRepository;
        _mapper = mapper;
    }
    public async Task<GetHotelResponse> Handle(GetHotelQuery request, CancellationToken cancellationToken)
    {
        var hotel = await _hotelRepository.GetByIdIncludeProperties(request.HotelId, h => h.Gallery.Where(g => g.ImageableId == h.Id));

        if (hotel == null)
        {
            throw new NotFoundException("Hotel not found");
        }

        return _mapper.Map<GetHotelResponse>(hotel);
    }
}
