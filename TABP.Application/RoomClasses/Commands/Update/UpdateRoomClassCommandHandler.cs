using AutoMapper;
using MediatR;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.RoomClasses.Commands.Update;
public class UpdateRoomClassCommandHandler : IRequestHandler<UpdateRoomClassCommand>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IRoomClassRepository _roomClassRepository;
    private readonly IAmenityRepository _amenityRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRoomClassCommandHandler(
        IHotelRepository hotelRepository,
        IRoomClassRepository roomClassRepository,
        IAmenityRepository amenityRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _hotelRepository = hotelRepository;
        _roomClassRepository = roomClassRepository;
        _amenityRepository = amenityRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    public async Task<Unit> Handle(UpdateRoomClassCommand request, CancellationToken cancellationToken = default)
    {
        var roomClass = await _roomClassRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(RoomClassExceptionMessages.NotFound);

        if (!await _hotelRepository.ExistsAsync(h => h.Id == request.HotelId, cancellationToken))
        {
            throw new NotFoundException(HotelExceptionMessages.NotFound);
        }

        var amenities = await _amenityRepository.GetAllByIdAsync(request.AmenityIds, cancellationToken);
        if (amenities.Count() != request.AmenityIds.Count())
        {
            throw new NotFoundException(AmenityExceptionMessages.NotFound);
        }
        _mapper.Map(request, roomClass);
        roomClass.Amenities = amenities.ToList();

        _roomClassRepository.Update(roomClass);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
