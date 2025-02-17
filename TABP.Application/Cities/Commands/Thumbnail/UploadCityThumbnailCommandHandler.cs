﻿using AutoMapper;
using MediatR;
using TABP.Application.Exceptions;
using TABP.Application.Exceptions.Messages;
using TABP.Domain.Entities;
using TABP.Domain.Enums;
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

    public async Task<Unit> Handle(UploadCityThumbnailCommand request, CancellationToken cancellationToken = default)
    {
        if (!await _cityRepository.ExistsAsync(c => c.Id == request.CityId, cancellationToken))
        {
            throw new NotFoundException(CityExceptionMessages.NotFound);
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var publicId = Guid.NewGuid().ToString();
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
