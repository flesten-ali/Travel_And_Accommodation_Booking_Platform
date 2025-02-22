using AutoMapper;
using MediatR;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Amenities.Commands.Update;
public class UpdateAmenityCommandHandler : IRequestHandler<UpdateAmenityCommand>
{
    private readonly IAmenityRepository _amenityRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAmenityCommandHandler(
        IAmenityRepository amenityRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _amenityRepository = amenityRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpdateAmenityCommand request, CancellationToken cancellationToken = default)
    {
        var amenity = await _amenityRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(AmenityExceptionMessages.NotFound);

        if (await _amenityRepository.ExistsAsync(a => a.Name == request.Name, cancellationToken))
        {
            throw new ConflictException(AmenityExceptionMessages.Exist);
        }

        _mapper.Map(request, amenity);

        _amenityRepository.Update(amenity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
