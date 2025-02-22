using MediatR;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Owners.Commands.Delete;
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
