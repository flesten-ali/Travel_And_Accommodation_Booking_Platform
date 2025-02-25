using AutoMapper;
using MediatR;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Amenities.Commands.Update;

/// <summary>
/// Handles the update of an existing amenity by processing an <see cref="UpdateAmenityCommand"/> request.
/// Implements <see cref="IRequestHandler{TRequest}"/> to handle the request asynchronously.
/// </summary>
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

    /// <summary>
    /// Handles the update of an amenity.
    /// </summary>
    /// <param name="request">The <see cref="UpdateAmenityCommand"/> containing updated amenity details.</param>
    /// <param name="cancellationToken">A cancellation token to observe while awaiting the asynchronous operation.</param>
    /// <returns>
    /// A task representing the asynchronous operation, returning <see cref="Unit.Value"/> when the operation completes successfully.
    /// </returns>
    /// <exception cref="NotFoundException">
    /// Thrown when the amenity with the specified ID is not found.
    /// </exception>
    /// <exception cref="ConflictException">
    /// Thrown when an amenity with the same name already exists.
    /// </exception>
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
