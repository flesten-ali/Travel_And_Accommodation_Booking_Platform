using AutoMapper;
using MediatR;
using TABP.Domain.Constants.ExceptionsMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence.Repositories;
namespace TABP.Application.Hotels.Queries.GetDetails;

public class GetHotelQueryHandler : IRequestHandler<GetHotelQuery, HotelDetailsResponse>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IMapper _mapper;

    public GetHotelQueryHandler(IHotelRepository hotelRepository, IMapper mapper)
    {
        _hotelRepository = hotelRepository;
        _mapper = mapper;
    }

    public async Task<HotelDetailsResponse> Handle(GetHotelQuery request, CancellationToken cancellationToken = default)
    {
        var hotel = await _hotelRepository.GetByIdIncludePropertiesAsync(request.HotelId, cancellationToken)
            ?? throw new NotFoundException(HotelExceptionMessages.NotFound);

        return _mapper.Map<HotelDetailsResponse>(hotel);
    }
}
