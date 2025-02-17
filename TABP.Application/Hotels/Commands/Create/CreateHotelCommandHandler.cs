using AutoMapper;
using MediatR;
using TABP.Application.Hotels.Common;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Entities;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Hotels.Commands.Create;
public class CreateHotelCommandHandler : IRequestHandler<CreateHotelCommand, HotelResponse>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly ICityRepository _cityRepository;
    private readonly IOwnerRepository _ownerRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CreateHotelCommandHandler(
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

    public async Task<HotelResponse> Handle(CreateHotelCommand request, CancellationToken cancellationToken = default)
    {
        var city = await _cityRepository.GetByIdAsync(request.CityId, cancellationToken)
            ?? throw new NotFoundException(CityExceptionMessages.NotFound);

        var owner = await _ownerRepository.GetByIdAsync(request.OwnerId, cancellationToken)
            ?? throw new NotFoundException(OwnerExceptionMessages.NotFound);

        if (await _hotelRepository.ExistsAsync(hotel =>
           hotel.LatitudeCoordinates == request.LatitudeCoordinates && hotel.LongitudeCoordinates == request.LongitudeCoordinates,
           cancellationToken))
        {
            throw new ExistsException(HotelExceptionMessages.ExistsInLocation);
        }

        var hotel = _mapper.Map<Hotel>(request);
        hotel.City = city;
        hotel.Owner = owner;

        await _hotelRepository.CreateAsync(hotel, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<HotelResponse>(hotel);
    }
}
