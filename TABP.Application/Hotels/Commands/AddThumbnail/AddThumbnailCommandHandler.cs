using AutoMapper;
using MediatR;
using TABP.Domain.Entities;
using TABP.Domain.Enums;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Interfaces.Services.Image;

namespace TABP.Application.Hotels.Commands.AddThumbnail;
public class AddThumbnailCommandHandler : IRequestHandler<AddThumbnailCommand, Guid>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IImageRepository _imageRepository;
    private readonly IImageUploadService _imageUploadService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddThumbnailCommandHandler(
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
    public async Task<Guid> Handle(AddThumbnailCommand request, CancellationToken cancellationToken)
    {
        if (!await _hotelRepository.ExistsAsync(h => h.Id == request.HotelId))
        {
            throw new NotFoundException("Hotel is not found");
        }

        var publicId = Guid.NewGuid().ToString();
        var imageUrl = await _imageUploadService.UploadAsync(request.Thumbnail, publicId);

        var image = _mapper.Map<Image>(request);
        image.ImageUrl = imageUrl;
        image.PublicId = publicId;

        await _imageRepository.DeleteByIdAsync(image.ImageableId, ImageType.Thumbnail);

        await _imageRepository.AddAsync(image);
        await _unitOfWork.SaveChangesAsync();
        return image.Id;
    }
}