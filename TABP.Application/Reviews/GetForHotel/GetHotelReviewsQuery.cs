using MediatR;
using TABP.Domain.Models;

namespace TABP.Application.Reviews.GetForHotel;
public class GetHotelReviewsQuery : IRequest<PaginatedList<HotelReviewsQueryReponse>>
{
    public Guid HotelId { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
}
