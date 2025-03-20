using AutoMapper;
using MediatR;
using TABP.Application.Reviews.Common;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Reviews.Queries.GetById;

/// <summary>
/// Handles the query to get a review by its ID for a specific hotel.
/// </summary>
public class GetReviewByIdQueryHandler : IRequestHandler<GetReviewByIdQuery, ReviewResponse>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IHotelRepository _hotelRepository;
    private readonly IMapper _mapper;

    public GetReviewByIdQueryHandler(
        IReviewRepository reviewRepository,
        IHotelRepository hotelRepository,
        IMapper mapper)
    {
        _reviewRepository = reviewRepository;
        _hotelRepository = hotelRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the request to retrieve a review by its ID for a specific hotel.
    /// </summary>
    /// <param name="request">The query containing the hotel ID and review ID to fetch the review.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>
    /// A task representing the asynchronous operation, returning a <see cref="ReviewResponse"/> containing review data.
    /// </returns>
    /// <exception cref="NotFoundException">
    /// Thrown if the specified hotel or review does not exist.
    /// </exception>
    public async Task<ReviewResponse> Handle(
        GetReviewByIdQuery request,
        CancellationToken cancellationToken = default)
    {
        if (!await _hotelRepository.ExistsAsync(h => h.Id == request.HotelId, cancellationToken))
        {
            throw new NotFoundException(HotelExceptionMessages.NotFound);
        }

        if (!await _reviewRepository
            .ExistsAsync(r => r.Id == request.ReviewId && r.HotelId == request.HotelId, cancellationToken))
        {
            throw new NotFoundException(ReviewExceptionMessages.NotFoundForHotel);
        }

        var review = await _reviewRepository.GetByIdAsync(request.ReviewId, cancellationToken);

        return _mapper.Map<ReviewResponse>(review);
    }
}
