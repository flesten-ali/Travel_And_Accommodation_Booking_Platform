using MediatR;
using TABP.Application.Shared;
using TABP.Domain.Models;

namespace TABP.Application.Reviews.Queries.GetForHotel;
public class GetHotelReviewsQuery : IRequest<PaginatedList<HotelReviewsQueryReponse>>
{
    public Guid HotelId { get; set; }
    public PaginationParameters PaginationParameters { get; set; }
}
