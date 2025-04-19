using MediatR;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Owners.Commands.Delete;

/// <summary>
/// Handles the command to delete an owner from the system.
/// </summary>
public class DeleteOwnerCommandHandler : IRequestHandler<DeleteOwnerCommand>
{
    private readonly IOwnerRepository _ownerRepository;
    private readonly IHotelRepository _hotelRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteOwnerCommandHandler(
        IOwnerRepository ownerRepository,
        IHotelRepository hotelRepository,
        IUnitOfWork unitOfWork)
    {
        _ownerRepository = ownerRepository;
        _hotelRepository = hotelRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Handles the request to delete an owner.
    /// </summary>
    /// <param name="request">The command containing the ID of the owner to be deleted.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>
    /// A task representing the asynchronous operation, returning <see cref="Unit.Value"/> upon successful deletion.
    /// </returns>
    /// <exception cref="NotFoundException">Thrown if the owner with the specified ID is not found.</exception>
    /// <exception cref="ConflictException">
    /// Thrown if the owner is associated with a hotel and cannot be deleted.
    /// </exception>
    public async Task<Unit> Handle(DeleteOwnerCommand request, CancellationToken cancellationToken = default)
    {
        if (!await _ownerRepository.ExistsAsync(o => o.Id == request.Id, cancellationToken))
        {
            throw new NotFoundException(OwnerExceptionMessages.NotFound);
        }

        if (await _hotelRepository.ExistsAsync(h => h.OwnerId == request.Id, cancellationToken))
        {
            throw new ConflictException(OwnerExceptionMessages.EntityInUse);
        }

        _ownerRepository.Delete(request.Id);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
