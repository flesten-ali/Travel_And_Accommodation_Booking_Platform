using AutoMapper;
using MediatR;
using TABP.Domain.Entities;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;
namespace TABP.Application.Amenities.Add;

public class AddAmenityCommandHandler : IRequestHandler<AddAmenityCommand, Guid>
{
    private readonly IAmenityRepository _amenityRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRoomClassRepository _roomClassRepository;

    public AddAmenityCommandHandler(IAmenityRepository amenityRepository, IMapper mapper, IUnitOfWork unitOfWork, IRoomClassRepository roomClassRepository)
    {
        _amenityRepository = amenityRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _roomClassRepository = roomClassRepository;
    }

    public async Task<Guid> Handle(AddAmenityCommand request, CancellationToken cancellationToken)
    {
        if (await _amenityRepository.ExistsAsync(a => a.Name == request.Name))
        {
            throw new ExistsException("Amenity is exists");
        }

        var amenity = _mapper.Map<Amenity>(request);

        await _amenityRepository.AddAsync(amenity);
        await _unitOfWork.SaveChangesAsync();

        return amenity.Id;
    }
}
