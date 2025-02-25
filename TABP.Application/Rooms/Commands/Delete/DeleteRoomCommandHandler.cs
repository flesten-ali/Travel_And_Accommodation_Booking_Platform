using MediatR;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Rooms.Commands.Delete;

/// <summary>
/// Handles the command to delete a room from a specific room class.
/// </summary>
public class DeleteRoomCommandHandler : IRequestHandler<DeleteRoomCommand>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBookingRepository _bookingRepository;
    private readonly IRoomClassRepository _roomClassRepository;

    public DeleteRoomCommandHandler(
        IRoomRepository roomRepository,
        IUnitOfWork unitOfWork,
        IBookingRepository bookingRepository,
        IRoomClassRepository roomClassRepository)
    {
        _roomRepository = roomRepository;
        _unitOfWork = unitOfWork;
        _bookingRepository = bookingRepository;
        _roomClassRepository = roomClassRepository;
    }

    /// <summary>
    /// Handles the request to delete a room from the specified room class.
    /// </summary>
    /// <param name="request">The command containing the room ID and room class ID to be deleted.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>A task representing the asynchronous operation, returning a unit value when completed.</returns>
    /// <exception cref="NotFoundException">Thrown if the room class or room does not exist.</exception>
    /// <exception cref="ConflictException">Thrown if the room is associated with any bookings.</exception>
    public async Task<Unit> Handle(DeleteRoomCommand request, CancellationToken cancellationToken = default)
    {
        if (!await _roomClassRepository.ExistsAsync(rc => rc.Id == request.RoomClassId, cancellationToken))
        {
            throw new NotFoundException(RoomClassExceptionMessages.NotFound);
        }

        if (!await _roomRepository
            .ExistsAsync(r => r.Id == request.RoomId && r.RoomClassId == request.RoomClassId, cancellationToken))
        {
            throw new NotFoundException(RoomExceptionMessages.NotFoundForTheRoomClass);
        }

        if (await _bookingRepository
            .ExistsAsync(b => b.Rooms.Any(r => r.Id == request.RoomId && r.RoomClassId == request.RoomClassId), cancellationToken))
        {
            throw new ConflictException(RoomExceptionMessages.EntityInUseForBookings);
        }

        _roomRepository.Delete(request.RoomId);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}