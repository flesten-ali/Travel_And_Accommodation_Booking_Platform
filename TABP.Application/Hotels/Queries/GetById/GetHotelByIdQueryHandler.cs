using AutoMapper;
using MediatR;
using TABP.Application.Hotels.Common;
using TABP.Domain.Constants.ExceptionsMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence.Repositories;
namespace TABP.Application.Hotels.Queries.GetById;

public class GetHotelByIdQueryHandler : IRequestHandler<GetHotelByIdQuery, HotelResponse>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IMapper _mapper;

    public GetHotelByIdQueryHandler(IHotelRepository hotelRepository, IMapper mapper)
    {
        _hotelRepository = hotelRepository;
        _mapper = mapper;
    }

    public async Task<HotelResponse> Handle(GetHotelByIdQuery request, CancellationToken cancellationToken = default)
    {
        var hotel = await _hotelRepository.GetByIdIncludePropertiesAsync(
            request.HotelId,
            cancellationToken,
            h => h.Owner, h => h.City)
            ?? throw new NotFoundException(HotelExceptionMessages.NotFound);

        return _mapper.Map<HotelResponse>(hotel);
    }
}
