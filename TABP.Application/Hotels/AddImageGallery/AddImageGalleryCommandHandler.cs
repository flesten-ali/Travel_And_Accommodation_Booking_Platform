using AutoMapper;
using MediatR;
using TABP.Domain.Entities;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Interfaces.Services;

namespace TABP.Application.Hotels.AddImageGallery;
internal class AddImageGalleryCommandHandler : IRequestHandler<AddImageGalleryCommand, Guid>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IImageRepository _imageRepository;
    private readonly IImageStorageService _imageStorageService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddImageGalleryCommandHandler(
        IHotelRepository hotelRepository,
        IImageRepository imageRepository,
        IImageStorageService imageStorageService,
        IUnitOfWork unitOfWork,
        IMapper mapper
    )
    {
        _hotelRepository = hotelRepository;
        _imageRepository = imageRepository;
        _imageStorageService = imageStorageService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(AddImageGalleryCommand request, CancellationToken cancellationToken)
    {
        if (!await _hotelRepository.ExistsAsync(h => h.Id == request.HotelId))
        {
            throw new NotFoundException("Hotel is not found");
        }
        var imageUrl = await _imageStorageService.UploadFileAsync(request.Image);

        var image = _mapper.Map<Image>(request);
        image.ImageUrl = imageUrl;

        await _imageRepository.AddAsync(image);
        await _unitOfWork.SaveChangesAsync();

        return image.Id;
    }
}
