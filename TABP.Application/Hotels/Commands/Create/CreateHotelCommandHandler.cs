using AutoMapper;
using MediatR;
using TABP.Application.Hotels.Common;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Entities;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Hotels.Commands.Create;

/// <summary>
/// Handles the creation of a new hotel by validating the input and saving it to the database.
/// </summary>
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

    /// <summary>
    /// Handles the creation of a hotel by validating the city, owner, and location, then saving the new hotel to the database.
    /// </summary>
    /// <param name="request">The command containing the data for creating a hotel.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation if needed.</param>
    /// <returns>A task representing the asynchronous operation, with a <see cref="HotelResponse"/> as the result.</returns>
    /// <exception cref="NotFoundException">Thrown if the city or owner does not exist in the database.</exception>
    /// <exception cref="ConflictException">Thrown if a hotel already exists at the given location (latitude and longitude).</exception>
    public async Task<HotelResponse> Handle(CreateHotelCommand request, CancellationToken cancellationToken = default)
    {
        if (!await _cityRepository.ExistsAsync(c => c.Id == request.CityId, cancellationToken))
        {
            throw new NotFoundException(CityExceptionMessages.NotFound);
        }

        if (!await _ownerRepository.ExistsAsync(o => o.Id == request.OwnerId, cancellationToken))
        {
            throw new NotFoundException(OwnerExceptionMessages.NotFound);
        }

        if (await _hotelRepository.ExistsAsync(hotel =>
           hotel.LatitudeCoordinates == request.LatitudeCoordinates && hotel.LongitudeCoordinates == request.LongitudeCoordinates,
           cancellationToken))
        {
            throw new ConflictException(HotelExceptionMessages.ExistsInLocation);
        }

        var hotel = _mapper.Map<Hotel>(request);

        await _hotelRepository.CreateAsync(hotel, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<HotelResponse>(hotel);
    }
}
