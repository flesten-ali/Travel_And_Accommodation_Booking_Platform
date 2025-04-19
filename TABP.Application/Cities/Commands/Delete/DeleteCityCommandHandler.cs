using MediatR;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Enums;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Cities.Commands.Delete;

/// <summary>
/// Handles the command to delete a city.
/// </summary>
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

    /// <summary>
    /// Handles the request to delete a city.
    /// </summary>
    /// <param name="request">The request containing the ID of the city to delete.</param>
    /// <param name="cancellationToken">The cancellation token for handling task cancellation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="NotFoundException">Thrown if the city does not exist.</exception>
    /// <exception cref="ConflictException">Thrown if the city is currently in use by any hotel.</exception>
    public async Task<Unit> Handle(DeleteCityCommand request, CancellationToken cancellationToken = default)
    {
        if (!await _cityRepository.ExistsAsync(c => c.Id == request.Id, cancellationToken))
        {
            throw new NotFoundException(CityExceptionMessages.NotFound);
        }

        if (await _hotelRepository.ExistsAsync(h => h.CityId == request.Id, cancellationToken))
        {
            throw new ConflictException(CityExceptionMessages.EntityInUse);
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
