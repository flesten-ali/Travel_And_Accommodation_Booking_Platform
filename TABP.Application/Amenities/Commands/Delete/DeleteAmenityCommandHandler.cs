using MediatR;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Amenities.Commands.Delete;
public class DeleteAmenityCommandHandler : IRequestHandler<DeleteAmenityCommand>
{
    private readonly IAmenityRepository _amenityRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAmenityCommandHandler(IAmenityRepository amenityRepository, IUnitOfWork unitOfWork)
    {
        _amenityRepository = amenityRepository;
        _unitOfWork = unitOfWork;
    }

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
