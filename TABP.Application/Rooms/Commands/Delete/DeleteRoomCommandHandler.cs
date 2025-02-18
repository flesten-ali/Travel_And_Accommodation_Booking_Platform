using MediatR;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Rooms.Commands.Delete;
public class DeleteRoomCommandHandler : IRequestHandler<DeleteRoomCommand>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBookingRepository _bookingRepository;

    public DeleteRoomCommandHandler(IRoomRepository roomRepository,
        IUnitOfWork unitOfWork,
        IBookingRepository bookingRepository)
    {
        _roomRepository = roomRepository;
        _unitOfWork = unitOfWork;
        _bookingRepository = bookingRepository;
    }

    public async Task<Unit> Handle(DeleteRoomCommand request, CancellationToken cancellationToken = default)
    {
        if (!await _roomRepository.ExistsAsync(r => r.Id == request.Id, cancellationToken))
        {
            throw new NotFoundException(RoomExceptionMessages.NotFound);
        }

        if (await _bookingRepository.ExistsAsync(b => b.Rooms.Any(r => r.Id == request.Id), cancellationToken))
        {
            throw new EntityInUseException(RoomExceptionMessages.EntityInUseForBookings);
        }

        _roomRepository.Delete(request.Id);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}