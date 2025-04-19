using AutoMapper;
using MediatR;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Reviews.Commands.Update;

/// <summary>
/// Handles the command to update an existing review for a hotel.
/// </summary>
internal class UpdateReviewCommandHandler : IRequestHandler<UpdateReviewCommand>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IUserRepository _userRepository;
    private readonly IReviewRepository _reviewRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateReviewCommandHandler(
        IHotelRepository hotelRepository,
        IUserRepository userRepository,
        IReviewRepository reviewRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _hotelRepository = hotelRepository;
        _userRepository = userRepository;
        _reviewRepository = reviewRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the request to update a review for a hotel.
    /// </summary>
    /// <param name="request">The command containing the data for the review to be updated.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>A task representing the asynchronous operation, returning a Unit value.</returns>
    /// <exception cref="NotFoundException">
    /// Thrown if the specified hotel, user, or review does not exist or the review does not belong 
    /// to the specified hotel and user.
    /// </exception>
    public async Task<Unit> Handle(UpdateReviewCommand request, CancellationToken cancellationToken = default)
    {
        if (!await _hotelRepository.ExistsAsync(h => h.Id == request.HotelId, cancellationToken))
        {
            throw new NotFoundException(HotelExceptionMessages.NotFound);
        }

        if (!await _userRepository.ExistsAsync(u => u.Id == request.UserId, cancellationToken))
        {
            throw new NotFoundException(UserExceptionMessages.NotFound);
        }

        if (!await _reviewRepository
            .ExistsAsync(r => r.Id == request.Id && r.HotelId == request.HotelId && r.UserId == request.UserId, cancellationToken))
        {
            throw new NotFoundException(ReviewExceptionMessages.NotFoundForHotel);
        }

        var review = await _reviewRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(ReviewExceptionMessages.NotFound);

        _mapper.Map(request, review);

        _reviewRepository.Update(review);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
