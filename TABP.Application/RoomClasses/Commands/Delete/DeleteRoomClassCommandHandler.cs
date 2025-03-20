using MediatR;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Enums;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.RoomClasses.Commands.Delete;

/// <summary>
/// Handles the command to delete a room class, ensuring that no rooms are using the class and any associated images are removed.
/// </summary>
public class DeleteRoomClassCommandHandler : IRequestHandler<DeleteRoomClassCommand>
{
    private readonly IRoomClassRepository _roomClassRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRoomRepository _roomRepository;
    private readonly IImageRepository _imageRepository;

    public DeleteRoomClassCommandHandler(
        IRoomClassRepository roomClassRepository,
        IUnitOfWork unitOfWork,
        IRoomRepository roomRepository,
        IImageRepository imageRepository)
    {
        _roomClassRepository = roomClassRepository;
        _unitOfWork = unitOfWork;
        _roomRepository = roomRepository;
        _imageRepository = imageRepository;
    }

    /// <summary>
    /// Handles the request to delete a room class, including validation and transaction management.
    /// </summary>
    /// <param name="request">The command containing the ID of the room class to be deleted.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>A task representing the asynchronous operation, returning
    /// <see cref="Unit.Value"/> on successful completion.</returns>
    /// <exception cref="NotFoundException">Thrown if the room class does not exist.</exception>
    /// <exception cref="ConflictException">Thrown if the room class is currently in use by one or more rooms.</exception>
    /// <exception cref="Exception">Thrown if the transaction cannot be completed successfully.</exception>
    public async Task<Unit> Handle(
        DeleteRoomClassCommand request,
        CancellationToken cancellationToken = default)
    {
        if (!await _roomClassRepository.ExistsAsync(rc => rc.Id == request.Id, cancellationToken))
        {
            throw new NotFoundException(RoomClassExceptionMessages.NotFound);
        }

        if (await _roomRepository.ExistsAsync(r => r.RoomClassId == request.Id, cancellationToken))
        {
            throw new ConflictException(RoomClassExceptionMessages.EntityInUseForRooms);
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            _roomClassRepository.Delete(request.Id);
            await _imageRepository.DeleteByIdAsync(request.Id, ImageType.Gallery, cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);

            return Unit.Value;
        }
        catch
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
