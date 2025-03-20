using MediatR;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Amenities.Commands.Delete;

/// <summary>
/// Handles the deletion of an amenity by processing a <see cref="DeleteAmenityCommand"/> request.
/// Implements <see cref="IRequestHandler{TRequest}"/> to handle the request asynchronously.
/// </summary>
public class DeleteAmenityCommandHandler : IRequestHandler<DeleteAmenityCommand>
{
    private readonly IAmenityRepository _amenityRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAmenityCommandHandler(IAmenityRepository amenityRepository, IUnitOfWork unitOfWork)
    {
        _amenityRepository = amenityRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Handles the deletion of an amenity.
    /// </summary>
    /// <param name="request">The <see cref="DeleteAmenityCommand"/> containing the ID of the amenity to be deleted.</param>
    /// <param name="cancellationToken">A cancellation token to observe while awaiting the asynchronous operation.</param>
    /// <returns>
    /// A task representing the asynchronous operation, returning <see cref="Unit.Value"/> when the operation completes successfully.
    /// </returns>
    /// <exception cref="NotFoundException">
    /// Thrown when the amenity with the specified ID is not found.
    /// </exception>
    public async Task<Unit> Handle(DeleteAmenityCommand request, CancellationToken cancellationToken = default)
    {
        if (!await _amenityRepository.ExistsAsync(a => a.Id == request.Id, cancellationToken))
        {
            throw new NotFoundException(AmenityExceptionMessages.NotFound);
        }

        _amenityRepository.Delete(request.Id);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
