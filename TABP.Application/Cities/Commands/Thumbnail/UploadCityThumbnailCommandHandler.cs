using AutoMapper;
using MediatR;
using TABP.Domain.Entities;
using TABP.Domain.Enums;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Interfaces.Services.Image;

namespace TABP.Application.Cities.Commands.Thumbnail;
public class UploadCityThumbnailCommandHandler : IRequestHandler<UploadCityThumbnailCommand>
{
    private readonly ICityRepository _cityRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageUploadService _imageUploadService;
    private readonly IMapper _mapper;
    private readonly IImageRepository _imageRepository;

    public UploadCityThumbnailCommandHandler(
        ICityRepository cityRepository,
        IUnitOfWork unitOfWork,
        IImageUploadService imageUploadService,
        IMapper mapper,
        IImageRepository imageRepository)
    {
        _cityRepository = cityRepository;
        _unitOfWork = unitOfWork;
        _imageUploadService = imageUploadService;
        _mapper = mapper;
        _imageRepository = imageRepository;
    }

    public async Task<Unit> Handle(UploadCityThumbnailCommand request, CancellationToken cancellationToken)
    {
        if (!await _cityRepository.ExistsAsync(c => c.Id == request.CityId))
        {
            throw new NotFoundException("City not found");
        }

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var publicId = Guid.NewGuid().ToString();
            var imageUrl = await _imageUploadService.UploadAsync(request.Thumbnail, publicId);

            var image = _mapper.Map<Image>(request);
            image.ImageUrl = imageUrl;
            image.PublicId = publicId;

            await _imageRepository.DeleteByIdAsync(request.CityId, ImageType.Thumbnail);
            await _imageRepository.AddAsync(image);

            await _unitOfWork.CommitAsync();

            return Unit.Value;
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }
}
