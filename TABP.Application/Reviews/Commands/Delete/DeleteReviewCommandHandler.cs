using MediatR;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Reviews.Commands.Delete;
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

        if (!await _reviewRepository.ExistsAsync(r => r.Id == request.ReviewId
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
