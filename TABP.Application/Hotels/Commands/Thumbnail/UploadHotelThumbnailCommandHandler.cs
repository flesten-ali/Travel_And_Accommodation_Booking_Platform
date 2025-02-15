using AutoMapper;
using MediatR;
using TABP.Domain.Entities;
using TABP.Domain.Enums;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Interfaces.Services.Image;

namespace TABP.Application.Hotels.Commands.Thumbnail;
public class UploadHotelThumbnailCommandHandler : IRequestHandler<UploadHotelThumbnailCommand>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IImageRepository _imageRepository;
    private readonly IImageUploadService _imageUploadService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UploadHotelThumbnailCommandHandler(
        IHotelRepository hotelRepository,
        IImageRepository imageRepository,
        IImageUploadService imageUploadService,
        IUnitOfWork unitOfWork,
        IMapper mapper
    )
    {
        _hotelRepository = hotelRepository;
        _imageRepository = imageRepository;
        _imageUploadService = imageUploadService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<Unit> Handle(UploadHotelThumbnailCommand request, CancellationToken cancellationToken)
    {
        if (!await _hotelRepository.ExistsAsync(h => h.Id == request.HotelId))
        {
            throw new NotFoundException("Hotel is not found");
        }

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var publicId = Guid.NewGuid().ToString();
            var imageUrl = await _imageUploadService.UploadAsync(request.Thumbnail, publicId);

            var image = _mapper.Map<Image>(request);
            image.ImageUrl = imageUrl;
            image.PublicId = publicId;

            await _imageRepository.DeleteByIdAsync(image.ImageableId, ImageType.Thumbnail);

            await _imageRepository.CreateAsync(image);
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