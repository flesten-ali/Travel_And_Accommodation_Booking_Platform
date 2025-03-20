using AutoMapper;
using MediatR;
using TABP.Application.Helpers;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Models;

namespace TABP.Application.Reviews.Queries.GetForHotel;

/// <summary>
/// Handles the query to retrieve a paginated list of reviews for a specific hotel.
/// </summary>
public class GetHotelReviewsQueryHandler
    : IRequestHandler<GetHotelReviewsQuery, PaginatedResponse<HotelReviewsQueryReponse>>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IMapper _mapper;

    public GetHotelReviewsQueryHandler(IReviewRepository reviewRepository, IMapper mapper)
    {
        _reviewRepository = reviewRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the request to retrieve a paginated list of reviews for a hotel.
    /// </summary>
    /// <param name="request">The query containing the hotel ID and pagination parameters to fetch the reviews.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>A task representing the asynchronous operation, returning a 
    /// <see cref="PaginatedResponse{HotelReviewsQueryReponse}"/> containing the paginated reviews.</returns>
    /// <exception cref="NotFoundException">Thrown if no reviews are found for the specified hotel.</exception>
    public async Task<PaginatedResponse<HotelReviewsQueryReponse>> Handle(
        GetHotelReviewsQuery request,
        CancellationToken cancellationToken = default)
    {
        var orderBy = SortBuilder.BuildReviewSort(request.PaginationParameters);

        var reviews = await _reviewRepository.GetByHotelIdAsync(
            orderBy,
            request.HotelId,
            request.PaginationParameters.PageSize,
            request.PaginationParameters.PageNumber,
            cancellationToken)
            ?? throw new NotFoundException(ReviewExceptionMessages.NotFound);

        return _mapper.Map<PaginatedResponse<HotelReviewsQueryReponse>>(reviews);
    }
}