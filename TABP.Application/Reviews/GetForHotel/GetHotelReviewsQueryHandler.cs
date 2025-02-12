using AutoMapper;
using MediatR;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Models;

namespace TABP.Application.Reviews.GetForHotel;
public class GetHotelReviewsQueryHandler
    : IRequestHandler<GetHotelReviewsQuery, PaginatedList<HotelReviewsQueryReponse>>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IMapper _mapper;

    public GetHotelReviewsQueryHandler(IReviewRepository reviewRepository, IMapper mapper)
    {
        _reviewRepository = reviewRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<HotelReviewsQueryReponse>> Handle(
        GetHotelReviewsQuery request,
        CancellationToken cancellationToken)
    {
        var reviews = await _reviewRepository.GetByHotelIdAsync(request.HotelId, request.PageSize, request.PageNumber)
              ?? throw new NotFoundException($"No reviews found for the hotel ID {request.HotelId}");

        return _mapper.Map<PaginatedList<HotelReviewsQueryReponse>>(reviews);
    }
}