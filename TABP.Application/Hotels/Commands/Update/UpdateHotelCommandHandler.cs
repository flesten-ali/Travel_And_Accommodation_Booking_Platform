using AutoMapper;
using MediatR;
using TABP.Domain.Constants.ExceptionMessages;
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
    public async Task<Unit> Handle(UpdateHotelCommand request, CancellationToken cancellationToken = default)
    {
        var hotel = await _hotelRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(HotelExceptionMessages.NotFound);

        if (!await _cityRepository.ExistsAsync(c => c.Id == request.CityId, cancellationToken))
        {
            throw new NotFoundException(CityExceptionMessages.NotFound);
        }

        if (!await _ownerRepository.ExistsAsync(o => o.Id == request.OwnerId, cancellationToken))
        {
            throw new NotFoundException(OwnerExceptionMessages.NotFound);
        }

        if (await _hotelRepository.ExistsAsync(hotel =>
          hotel.LatitudeCoordinates == request.LatitudeCoordinates
          && hotel.LongitudeCoordinates == request.LongitudeCoordinates,
          cancellationToken))
        {
            throw new ConflictException(HotelExceptionMessages.ExistsInLocation);
        }

        _mapper.Map(request, hotel);

        _hotelRepository.Update(hotel);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
