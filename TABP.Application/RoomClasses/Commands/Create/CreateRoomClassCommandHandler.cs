using AutoMapper;
using MediatR;
using TABP.Application.RoomClasses.Common;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Entities;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.RoomClasses.Commands.Create;

/// <summary>
/// Handles the command to create a new room class for a specific hotel with associated amenities.
/// </summary>
public class CreateRoomClassCommandHandler : IRequestHandler<CreateRoomClassCommand, RoomClassResponse>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IAmenityRepository _amenityRepository;
    private readonly IRoomClassRepository _roomClassRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateRoomClassCommandHandler(
        IHotelRepository hotelRepository,
        IAmenityRepository amenityRepository,
        IRoomClassRepository roomClassRepository,
        IUnitOfWork unitOfWork,
         IMapper mapper)
    {
        _hotelRepository = hotelRepository;
        _amenityRepository = amenityRepository;
        _roomClassRepository = roomClassRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the request to create a new room class for a hotel, including validation and mapping.
    /// </summary>
    /// <param name="request">The command containing the hotel ID, room class details, and amenity IDs.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>A task representing the asynchronous operation, returning a 
    /// <see cref="RoomClassResponse"/> containing the created room class details.</returns>
    /// <exception cref="NotFoundException">Thrown if the hotel or any of the amenities do not exist.</exception>
    public async Task<RoomClassResponse> Handle(
        CreateRoomClassCommand request,
        CancellationToken cancellationToken = default)
    {
        if (!await _hotelRepository.ExistsAsync(h => h.Id == request.HotelId, cancellationToken))
        {
            throw new NotFoundException(HotelExceptionMessages.NotFound);
        }

        var amenities = await _amenityRepository.GetAllByIdAsync(
            request.AmenityIds,
            cancellationToken);

        if (amenities.Count() != request.AmenityIds.Count())
        {
            throw new NotFoundException(AmenityExceptionMessages.NotFound);
        }

        var roomClass = _mapper.Map<RoomClass>(request);
        roomClass.Amenities = amenities.ToList();

        await _roomClassRepository.CreateAsync(roomClass, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<RoomClassResponse>(roomClass);
    }
}
