using AutoMapper;
using MediatR;
using TABP.Application.Amenities.Common;
using TABP.Domain.Constants.ExceptionsMessages;
using TABP.Domain.Entities;
using TABP.Domain.Exceptions;
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
        IUnitOfWork unitOfWork)
    {
        _amenityRepository = amenityRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<AmenityResponse> Handle(
        CreateAmenityCommand request,
        CancellationToken cancellationToken = default)
    {
        if (await _amenityRepository.ExistsAsync(a => a.Name == request.Name, cancellationToken))
        {
            throw new ExistsException(AmenityExceptionMessages.Exist);
        }

        var amenity = _mapper.Map<Amenity>(request);

        await _amenityRepository.CreateAsync(amenity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<AmenityResponse>(amenity);
    }
}
