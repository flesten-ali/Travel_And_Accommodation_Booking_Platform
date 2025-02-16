using AutoMapper;
using MediatR;
using TABP.Application.Exceptions;
using TABP.Application.Exceptions.Messages;
using TABP.Application.Hotels.Common;
using TABP.Domain.Entities;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Hotels.Commands.Create;
public class CreateHotelCommandHandler : IRequestHandler<CreateHotelCommand, HotelResponse>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly ICityRepository _cityRepository;
    private readonly IOwnerRepository _ownerRepository;
    private readonly IImageRepository _imageRepository;
    private readonly IRoomClassRepository _roomClassRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CreateHotelCommandHandler(
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

    public async Task<HotelResponse> Handle(CreateHotelCommand request, CancellationToken cancellationToken)
    {
        var city = await _cityRepository.GetByIdAsync(request.CityId)
            ?? throw new NotFoundException(CityExceptionMessages.NotFound);

        var owner = await _ownerRepository.GetByIdAsync(request.OwnerId)
            ?? throw new NotFoundException(OwnerExceptionMessages.NotFound);

        if (await _hotelRepository.ExistsAsync(hotel =>
           hotel.LatitudeCoordinates == request.LatitudeCoordinates && hotel.LongitudeCoordinates == request.LongitudeCoordinates))
        {
            throw new ExistsException(HotelExceptionMessages.ExistsInLocation);
        }

        var hotel = _mapper.Map<Hotel>(request);
        hotel.City = city;
        hotel.Owner = owner;

        await _hotelRepository.CreateAsync(hotel);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<HotelResponse>(hotel);
    }
}
