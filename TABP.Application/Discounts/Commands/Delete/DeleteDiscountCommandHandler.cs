using MediatR;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Discounts.Commands.Delete;

/// <summary>
/// Handles the command for deleting a discount for a specific room class.
/// </summary>
public class DeleteDiscountCommandHandler : IRequestHandler<DeleteDiscountCommand>
{
    private readonly IRoomClassRepository _roomClassRepository;
    private readonly IDiscountRepository _discountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteDiscountCommandHandler(
        IRoomClassRepository roomClassRepository,
        IDiscountRepository discountRepository,
        IUnitOfWork unitOfWork)
    {
        _roomClassRepository = roomClassRepository;
        _discountRepository = discountRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Handles the deletion of a discount for a specific room class.
    /// </summary>
    /// <param name="request">The request containing the details of the discount to be deleted.</param>
    /// <param name="cancellationToken">A cancellation token for gracefully canceling the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="NotFoundException">Thrown if the room class or the discount does not exist.</exception>
    public async Task<Unit> Handle(DeleteDiscountCommand request, CancellationToken cancellationToken = default)
    {
        if (!await _roomClassRepository.ExistsAsync(rc => rc.Id == request.RoomClassId, cancellationToken))
        {
            throw new NotFoundException(RoomClassExceptionMessages.NotFound);
        }

        if (!await _discountRepository
            .ExistsAsync(d => d.Id == request.DiscountId && d.RoomClassId == request.RoomClassId, cancellationToken))
        {
            throw new NotFoundException(DiscountExceptionMessages.NotFoundForTheRoomClass);
        }

        _discountRepository.Delete(request.DiscountId);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
