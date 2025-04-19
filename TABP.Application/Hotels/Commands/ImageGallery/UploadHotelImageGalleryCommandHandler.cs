using AutoMapper;
using MediatR;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Entities;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Interfaces.Services.Guids;
using TABP.Domain.Interfaces.Services.Image;

namespace TABP.Application.Hotels.Commands.ImageGallery;

/// <summary>
/// Handles the process of uploading a hotel image to the gallery, ensuring that the hotel exists, 
/// uploading the image, creating an entry for the image in the repository, and managing transactions.
/// </summary>
internal class UploadHotelImageGalleryCommandHandler : IRequestHandler<UploadHotelImageGalleryCommand>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IImageRepository _imageRepository;
    private readonly IImageUploadService _imageUploadService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IGuidProvider _guidProvider;

    public UploadHotelImageGalleryCommandHandler(
        IHotelRepository hotelRepository,
        IImageRepository imageRepository,
        IImageUploadService imageUploadService,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IGuidProvider guidProvider
    )
    {
        _hotelRepository = hotelRepository;
        _imageRepository = imageRepository;
        _imageUploadService = imageUploadService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _guidProvider = guidProvider;
    }

    /// <summary>
    /// Handles the uploading of an image to the hotel gallery.
    /// Validates that the hotel exists, uploads the image to the external service,
    /// creates a record for the image in the database, and commits the transaction.
    /// </summary>
    /// <param name="request">The command containing the data necessary to upload the image (hotel ID and image data).</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation if needed.</param>
    /// <returns>A task representing the asynchronous operation. The result is of type <see cref="Unit"/>.</returns>
    /// <exception cref="NotFoundException">Thrown if the hotel with the given ID does not exist.</exception>
    /// <exception cref="Exception">
    /// Thrown if any error occurs during the image upload or transaction process, causing a rollback.
    /// </exception>
    public async Task<Unit> Handle(UploadHotelImageGalleryCommand request, CancellationToken cancellationToken = default)
    {
        if (!await _hotelRepository.ExistsAsync(h => h.Id == request.HotelId, cancellationToken))
        {
            throw new NotFoundException(HotelExceptionMessages.NotFound);
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var publicId = _guidProvider.NewGuid().ToString();
            var imageUrl = await _imageUploadService.UploadAsync(request.Image, publicId, cancellationToken);

            var image = _mapper.Map<Image>(request);
            image.ImageUrl = imageUrl;
            image.PublicId = publicId;

            await _imageRepository.CreateAsync(image, cancellationToken);
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
