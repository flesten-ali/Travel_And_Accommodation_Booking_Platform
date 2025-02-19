using MediatR;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Discounts.Commands.Delete;
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
