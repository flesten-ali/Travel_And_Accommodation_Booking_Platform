using MediatR;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Bookings.Commands.Delete;

/// <summary>
/// Handles the deletion of a booking.
/// </summary>
public class DeleteBookingCommandHandler : IRequestHandler<DeleteBookingCommand>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;

    public DeleteBookingCommandHandler(
        IBookingRepository bookingRepository,
        IUnitOfWork unitOfWork,
        IUserRepository userRepository)
    {
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
    }

    /// <summary>
    /// Handles the request to delete a booking.
    /// </summary>
    /// <param name="request">The delete booking command containing the user and booking IDs.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Unit"/> indicating successful completion.</returns>
    /// <exception cref="NotFoundException">
    /// Thrown if the user or booking does not exist.
    /// </exception>
    public async Task<Unit> Handle(DeleteBookingCommand request, CancellationToken cancellationToken = default)
    {
        if (!await _userRepository.ExistsAsync(u => u.Id == request.UserId, cancellationToken))
        {
            throw new NotFoundException(UserExceptionMessages.NotFound);
        }

        if (!await _bookingRepository
            .ExistsAsync(b => b.Id == request.BookingId && b.UserId == request.UserId, cancellationToken))
        {
            throw new NotFoundException(BookingExceptionMessages.NotFoundForUser);
        }

        _bookingRepository.Delete(request.BookingId);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
