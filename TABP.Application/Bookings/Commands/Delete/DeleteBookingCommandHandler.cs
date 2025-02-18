using MediatR;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Bookings.Commands.Delete;
public class DeleteBookingCommandHandler : IRequestHandler<DeleteBookingCommand>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBookingCommandHandler(IBookingRepository bookingRepository, IUnitOfWork unitOfWork)
    {
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteBookingCommand request, CancellationToken cancellationToken = default)
    {
        if (!await _bookingRepository.ExistsAsync(b => b.Id == request.Id, cancellationToken))
        {
            throw new NotFoundException(BookingExceptionMessages.NotFound);
        }

        _bookingRepository.Delete(request.Id);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
