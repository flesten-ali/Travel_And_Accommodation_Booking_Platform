﻿using AutoMapper;
using MediatR;
using TABP.Domain.Entities;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Hotels.Add;
public class AddHotelCommandHandler : IRequestHandler<AddHotelCommand, Guid>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly ICityRepository _cityRepository;
    private readonly IOwnerRepository _ownerRepository;
    private readonly IImageRepository _imageRepository;
    private readonly IRoomClassRepository _roomClassRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public AddHotelCommandHandler(
        IHotelRepository hotelRepository,
        ICityRepository cityRepository,
        IOwnerRepository ownerRepository,
        IImageRepository imageRepository,
        IRoomClassRepository roomClassRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _hotelRepository = hotelRepository;
        _cityRepository = cityRepository;
        _ownerRepository = ownerRepository;
        _imageRepository = imageRepository;
        _roomClassRepository = roomClassRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(AddHotelCommand request, CancellationToken cancellationToken)
    {
        if (!await _cityRepository.ExistsAsync(city => city.Id == request.CityId))
        {
            throw new NotFoundException("City not found");
        }

        if (!await _ownerRepository.ExistsAsync(owner => owner.Id == request.OwnerId))
        {
            throw new NotFoundException("Owner not found");
        }

        if (!await _hotelRepository.ExistsAsync(hotel =>
           hotel.LatitudeCoordinates == request.LatitudeCoordinates && hotel.LongitudeCoordinates == request.LongitudeCoordinates))
        {
            throw new ExistsException("Hotel is exists in the provided location");
        }

        var hotel = _mapper.Map<Hotel>(request);

        await _hotelRepository.AddAsync(hotel);
        await _unitOfWork.SaveChangesAsync();

        return hotel.Id;
    }
}
