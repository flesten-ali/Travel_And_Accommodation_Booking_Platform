using AutoMapper;
using MediatR;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Entities;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Interfaces.Services.Guids;
using TABP.Domain.Interfaces.Services.Image;

namespace TABP.Application.RoomClasses.Commands.ImageGallery;

/// <summary>
/// Handles the command to upload an image to the gallery for a specific room class.
/// </summary>
public class UploadRoomClassImageGalleryCommandHandler
    : IRequestHandler<UploadRoomClassImageGalleryCommand>
{
    private readonly IRoomClassRepository _roomClassRepository;
    private readonly IImageRepository _imageRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageUploadService _imageUploadService;
    private readonly IGuidProvider _guidProvider;
    private readonly IMapper _mapper;

    public UploadRoomClassImageGalleryCommandHandler(
        IRoomClassRepository roomClassRepository,
        IImageRepository imageRepository,
        IUnitOfWork unitOfWork,
        IImageUploadService imageUploadService,
        IGuidProvider guidProvider,
        IMapper mapper)
    {
        _roomClassRepository = roomClassRepository;
        _imageRepository = imageRepository;
        _unitOfWork = unitOfWork;
        _imageUploadService = imageUploadService;
        _guidProvider = guidProvider;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the request to upload an image to the gallery for a room class, ensuring the room class exists
    /// and managing the transaction for image upload and storage.
    /// </summary>
    /// <param name="request">The command containing the room class ID and image data.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>A task representing the asynchronous operation, returning 
    /// <see cref="Unit.Value"/> on successful completion.</returns>
    /// <exception cref="NotFoundException">Thrown if the room class does not exist.</exception>
    /// <exception cref="Exception">Thrown if the transaction cannot be completed successfully.</exception>
    public async Task<Unit> Handle(
        UploadRoomClassImageGalleryCommand request, 
        CancellationToken cancellationToken = default)
    {
        if (!await _roomClassRepository.ExistsAsync(rc => rc.Id == request.RoomClassId, cancellationToken))
        {
            throw new NotFoundException(RoomClassExceptionMessages.NotFound);
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var publicId = _guidProvider.NewGuid().ToString();

            var imageUrl = await _imageUploadService.UploadAsync(
                request.Image,
                publicId, 
                cancellationToken);

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
