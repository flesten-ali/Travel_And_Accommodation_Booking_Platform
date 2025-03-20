using MediatR;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Reviews.Commands.Delete;

/// <summary>
/// Handles the command to delete a review for a hotel.
/// </summary>
public class DeleteReviewCommandHandler : IRequestHandler<DeleteReviewCommand>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IUserRepository _userRepository;
    private readonly IReviewRepository _reviewRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteReviewCommandHandler(
        IHotelRepository hotelRepository,
        IUserRepository userRepository,
        IReviewRepository reviewRepository,
        IUnitOfWork unitOfWork)
    {
        _hotelRepository = hotelRepository;
        _userRepository = userRepository;
        _reviewRepository = reviewRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Handles the request to delete a review for a hotel.
    /// </summary>
    /// <param name="request">The command containing the data for the review to be deleted.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>A task representing the asynchronous operation, returning a Unit value.</returns>
    /// <exception cref="NotFoundException">
    /// Thrown if the specified hotel, user, or review does not exist or the review does not belong
    /// to the specified hotel and user.
    /// </exception>
    public async Task<Unit> Handle(DeleteReviewCommand request, CancellationToken cancellationToken = default)
    {
        if (!await _hotelRepository.ExistsAsync(h => h.Id == request.HotelId, cancellationToken))
        {
            throw new NotFoundException(HotelExceptionMessages.NotFound);
        }

        if (!await _userRepository.ExistsAsync(u => u.Id == request.UserId, cancellationToken))
        {
            throw new NotFoundException(UserExceptionMessages.NotFound);
        }

        if (!await _reviewRepository.ExistsAsync(r =>
               r.Id == request.ReviewId
            && r.HotelId == request.HotelId
            && r.UserId == request.UserId, cancellationToken))
        {
            throw new NotFoundException(ReviewExceptionMessages.NotFoundForHotel);
        }

        _reviewRepository.Delete(request.ReviewId);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
