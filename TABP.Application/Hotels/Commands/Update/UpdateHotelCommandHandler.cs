using AutoMapper;
using MediatR;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Hotels.Commands.Update;

/// <summary>
/// Handles the process of updating a hotel’s details, ensuring that the hotel exists,
/// the city and owner are valid, there are no conflicts in location, and then saving the updated hotel information.
/// </summary>
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

    /// <summary>
    /// Handles the update operation for a hotel, validating the hotel, city, owner, and location,
    /// then updating the hotel information and saving it.
    /// </summary>
    /// <param name="request">The command containing the updated hotel data.</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the operation if needed.</param>
    /// <returns>A task representing the asynchronous operation. The result is of type <see cref="Unit"/>.</returns>
    /// <exception cref="NotFoundException">Thrown if the hotel, city, or owner is not found in the repository.</exception>
    /// <exception cref="ConflictException">Thrown if there is a conflict with the hotel’s location.</exception>
    /// <exception cref="Exception">
    /// Thrown if an error occurs during the update or transaction process, causing a rollback.
    /// </exception>
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
          hotel.LatitudeCoordinates == request.LatitudeCoordinates && hotel.LongitudeCoordinates == request.LongitudeCoordinates,
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
