using AutoMapper;
using MediatR;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Hotels.Commands.Update;
public class UpdateHotelCommandHandler : IRequestHandler<UpdateHotelCommand>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly ICityRepository _cityRepository;
    private readonly IOwnerRepository _ownerRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateHotelCommandHandler(
        IHotelRepository hotelRepository,
        ICityRepository cityRepository,
        IOwnerRepository ownerRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _hotelRepository = hotelRepository;
        _cityRepository = cityRepository;
        _ownerRepository = ownerRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    public async Task<Unit> Handle(UpdateHotelCommand request, CancellationToken cancellationToken)
    {
        var hotel = await _hotelRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException("Hotel not found");

        if (!await _cityRepository.ExistsAsync(c => c.Id == request.CityId))
        {
            throw new NotFoundException("City not found");
        }

        if (!await _ownerRepository.ExistsAsync(o => o.Id == request.OwnerId))
        {
            throw new NotFoundException("Owner not found");
        }

        if (await _hotelRepository.ExistsAsync(hotel =>
          hotel.LatitudeCoordinates == request.LatitudeCoordinates
          && hotel.LongitudeCoordinates == request.LongitudeCoordinates))
        {
            throw new ExistsException("Hotel is exists in the provided location");
        }

        _mapper.Map(request, hotel);

        _hotelRepository.Update(hotel);
        await _unitOfWork.SaveChangesAsync();

        return Unit.Value;
    }
}
