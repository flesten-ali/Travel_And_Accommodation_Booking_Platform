using AutoMapper;
using MediatR;
using TABP.Application.Exceptions;
using TABP.Application.Exceptions.Messages;
using TABP.Application.Helper;
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

        return _mapper.Map<PaginatedList<HotelReviewsQueryReponse>>(reviews);
    }
}