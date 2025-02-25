using AutoMapper;
using MediatR;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Entities;
using TABP.Domain.Enums;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Interfaces.Services.Guids;
using TABP.Domain.Interfaces.Services.Image;

namespace TABP.Application.Cities.Commands.Thumbnail;

/// <summary>
/// Handles the command to upload a new thumbnail for a city.
/// </summary>
public class UploadCityThumbnailCommandHandler : IRequestHandler<UploadCityThumbnailCommand>
{
    private readonly ICityRepository _cityRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageUploadService _imageUploadService;
    private readonly IMapper _mapper;
    private readonly IImageRepository _imageRepository;
    private readonly IGuidProvider _guidProvider;

    public UploadCityThumbnailCommandHandler(
        ICityRepository cityRepository,
        IUnitOfWork unitOfWork,
        IImageUploadService imageUploadService,
        IMapper mapper,
        IImageRepository imageRepository,
        IGuidProvider guidProvider)
    {
        _cityRepository = cityRepository;
        _unitOfWork = unitOfWork;
        _imageUploadService = imageUploadService;
        _mapper = mapper;
        _imageRepository = imageRepository;
        _guidProvider = guidProvider;
    }

    /// <summary>
    /// Handles the request to upload a new thumbnail image for a city.
    /// </summary>
    /// <param name="request">The request containing the city ID and thumbnail image.</param>
    /// <param name="cancellationToken">The cancellation token for handling task cancellation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="NotFoundException">Thrown if the city does not exist.</exception>
    public async Task<Unit> Handle(UploadCityThumbnailCommand request, CancellationToken cancellationToken = default)
    {
        if (!await _cityRepository.ExistsAsync(c => c.Id == request.CityId, cancellationToken))
        {
            throw new NotFoundException(CityExceptionMessages.NotFound);
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var publicId = _guidProvider.NewGuid().ToString();
            var imageUrl = await _imageUploadService.UploadAsync(request.Thumbnail, publicId, cancellationToken);

            var image = _mapper.Map<Image>(request);
            image.ImageUrl = imageUrl;
            image.PublicId = publicId;

            await _imageRepository.DeleteByIdAsync(request.CityId, ImageType.Thumbnail, cancellationToken);
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
