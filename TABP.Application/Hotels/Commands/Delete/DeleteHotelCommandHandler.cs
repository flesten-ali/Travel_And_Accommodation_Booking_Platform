using MediatR;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Enums;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Hotels.Commands.Delete;

/// <summary>
/// Handles the deletion of a hotel by validating the hotel existence and ensuring there are no associated room classes,
/// then performing the deletion while also handling the related image deletions (e.g., thumbnail and gallery images).
/// </summary>
public class DeleteHotelCommandHandler : IRequestHandler<DeleteHotelCommand>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IRoomClassRepository _roomClassRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageRepository _imageRepository;

    public DeleteHotelCommandHandler(
        IHotelRepository hotelRepository,
        IRoomClassRepository roomClassRepository,
        IUnitOfWork unitOfWork,
        IImageRepository imageRepository)
    {
        _hotelRepository = hotelRepository;
        _roomClassRepository = roomClassRepository;
        _unitOfWork = unitOfWork;
        _imageRepository = imageRepository;
    }

    /// <summary>
    /// Handles the deletion of a hotel by validating the existence of the hotel and checking if any room classes are associated with it.
    /// Deletes the hotel and its associated images, and commits the transaction.
    /// </summary>
    /// <param name="request">The command containing the hotel ID to be deleted.</param>
    /// <param name="cancellationToken">The cancellation token for cancelling the operation if necessary.</param>
    /// <returns>A task representing the asynchronous operation. The result is of type <see cref="Unit"/>.</returns>
    /// <exception cref="NotFoundException">Thrown if the hotel does not exist.</exception>
    /// <exception cref="ConflictException">Thrown if the hotel has associated room classes.</exception>
    /// <exception cref="Exception">Thrown if an error occurs during the operation, causing a rollback of the transaction.</exception>
    public async Task<Unit> Handle(DeleteHotelCommand request, CancellationToken cancellationToken = default)
    {
        if (!await _hotelRepository.ExistsAsync(h => h.Id == request.Id, cancellationToken))
        {
            throw new NotFoundException(HotelExceptionMessages.NotFound);
        }

        if (await _roomClassRepository.ExistsAsync(rc => rc.HotelId == request.Id, cancellationToken))
        {
            throw new ConflictException(HotelExceptionMessages.EntityInUseForRoomClasses);
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await _imageRepository.DeleteByIdAsync(request.Id, ImageType.Thumbnail, cancellationToken);
            await _imageRepository.DeleteByIdAsync(request.Id, ImageType.Gallery, cancellationToken);

            _hotelRepository.Delete(request.Id);

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
