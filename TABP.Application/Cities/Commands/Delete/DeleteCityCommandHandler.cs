using MediatR;
using TABP.Domain.Constants.ExceptionsMessages;
using TABP.Domain.Enums;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;
namespace TABP.Application.Cities.Commands.Delete;

public class DeleteCityCommandHandler : IRequestHandler<DeleteCityCommand>
{
    private readonly ICityRepository _cityRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageRepository _imageRepository;
    private readonly IHotelRepository _hotelRepository;

    public DeleteCityCommandHandler(
        ICityRepository cityRepository,
        IUnitOfWork unitOfWork,
        IImageRepository imageRepository,
        IHotelRepository hotelRepository)
    {
        _cityRepository = cityRepository;
        _unitOfWork = unitOfWork;
        _imageRepository = imageRepository;
        _hotelRepository = hotelRepository;
    }

    public async Task<Unit> Handle(DeleteCityCommand request, CancellationToken cancellationToken = default)
    {
        if (!await _cityRepository.ExistsAsync(c => c.Id == request.Id, cancellationToken))
        {
            throw new NotFoundException(CityExceptionMessages.NotFound);
        }

        if (await _hotelRepository.ExistsAsync(h => h.CityId == request.Id, cancellationToken))
        {
            throw new EntityInUseException(CityExceptionMessages.EntityInUse);
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await _imageRepository.DeleteByIdAsync(request.Id, ImageType.Thumbnail, cancellationToken);
            _cityRepository.Delete(request.Id);

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
