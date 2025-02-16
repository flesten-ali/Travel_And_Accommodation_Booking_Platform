using AutoMapper;
using MediatR;
using TABP.Application.Exceptions;
using TABP.Application.Exceptions.Messages;
using TABP.Application.Shared;
using TABP.Domain.Entities;
using TABP.Domain.Enums;
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
        var orderBy = BuildSort(request.PaginationParameters);
        var reviews = await _reviewRepository.GetByHotelIdAsync(
            orderBy,
            request.HotelId,
            request.PaginationParameters.PageSize,
            request.PaginationParameters.PageNumber)
            ?? throw new NotFoundException(ReviewExceptionMessages.NotFound);

        return _mapper.Map<PaginatedList<HotelReviewsQueryReponse>>(reviews);
    }

    private static Func<IQueryable<Review>, IOrderedQueryable<Review>> BuildSort(PaginationParameters paginationParameters)
    {
        var isDescending = paginationParameters.SortOrder == SortOrder.Descending;
        return paginationParameters.OrderColumn switch
        {
            "date" => isDescending
                    ? (reviews) => reviews.OrderByDescending(x => x.CreatedAt)
                    : (reviews) => reviews.OrderBy(x => x.CreatedAt),

            _ => (reviews) => reviews.OrderBy(h => h.Id)
        };
    }
}