using AutoMapper;
using MediatR;
using TABP.Application.Amenities.Common;
using TABP.Application.Exceptions;
using TABP.Application.Exceptions.Messages;
using TABP.Domain.Entities;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;
namespace TABP.Application.Amenities.Commands.Create;

public class CreateAmenityCommandHandler : IRequestHandler<CreateAmenityCommand, AmenityResponse>
{
    private readonly IAmenityRepository _amenityRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAmenityCommandHandler(
        IAmenityRepository amenityRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IRoomClassRepository roomClassRepository)
    {
        _amenityRepository = amenityRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<AmenityResponse> Handle(CreateAmenityCommand request, CancellationToken cancellationToken)
    {
        if (await _amenityRepository.ExistsAsync(a => a.Name == request.Name))
        {
            throw new ExistsException(AmenityExceptionMessages.NotFound);
        }

        var amenity = _mapper.Map<Amenity>(request);

        await _amenityRepository.CreateAsync(amenity);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<AmenityResponse>(amenity);
    }
}
