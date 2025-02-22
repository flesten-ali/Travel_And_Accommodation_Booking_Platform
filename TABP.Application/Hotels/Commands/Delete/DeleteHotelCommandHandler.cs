using MediatR;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Enums;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Hotels.Commands.Delete;
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
