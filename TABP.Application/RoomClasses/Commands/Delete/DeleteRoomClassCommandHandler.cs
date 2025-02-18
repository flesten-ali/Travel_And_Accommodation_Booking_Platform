using MediatR;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Enums;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.RoomClasses.Commands.Delete;
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

    public async Task<Unit> Handle(DeleteRoomClassCommand request, CancellationToken cancellationToken = default)
    {
        if (!await _roomClassRepository.ExistsAsync(rc => rc.Id == request.Id, cancellationToken))
        {
            throw new NotFoundException(RoomClassExceptionMessages.NotFound);
        }

        if (await _roomRepository.ExistsAsync(r => r.RoomClassId == request.Id, cancellationToken))
        {
            throw new NotFoundException(RoomClassExceptionMessages.EntityInUseForRooms);
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            _roomClassRepository.Delete(request.Id);
            await _imageRepository.DeleteByIdAsync(request.Id, ImageType.Gallery, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
        catch
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
        return Unit.Value;
    }
}
