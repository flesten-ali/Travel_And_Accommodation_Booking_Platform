using AutoMapper;
using MediatR;
using TABP.Application.Amenities.Common;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Entities;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Amenities.Commands.Create;

/// <summary>
/// Handles the creation of a new amenity by processing a <see cref="CreateAmenityCommand"/> request.
/// Implements <see cref="IRequestHandler{TRequest, TResponse}"/> to handle the request asynchronously.
/// </summary>
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

    /// <summary>
    /// Handles the creation of an amenity.
    /// </summary>
    /// <param name="request">The <see cref="CreateAmenityCommand"/> containing the new amenity details.</param>
    /// <param name="cancellationToken">A cancellation token to observe while awaiting the asynchronous operation.</param>
    /// <returns>
    /// A task representing the asynchronous operation, returning an <see cref="AmenityResponse"/> containing the newly created amenity details.
    /// </returns>
    /// <exception cref="ConflictException">
    /// Thrown when an amenity with the same name already exists in the system.
    /// </exception>
    public async Task<AmenityResponse> Handle(
        CreateAmenityCommand request,
        CancellationToken cancellationToken = default)
    {
        if (await _amenityRepository.ExistsAsync(a => a.Name == request.Name, cancellationToken))
        {
            throw new ConflictException(AmenityExceptionMessages.Exist);
        }

        var amenity = _mapper.Map<Amenity>(request);

        await _amenityRepository.CreateAsync(amenity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<AmenityResponse>(amenity);
    }
}
