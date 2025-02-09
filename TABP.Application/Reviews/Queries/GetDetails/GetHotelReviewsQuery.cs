using MediatR;
using TABP.Domain.Models;

namespace TABP.Application.Reviews.Queries.GetDetails;
public class GetHotelReviewsQuery : IRequest<PaginatedList<GetHotelReviewsQueryReponse>>
{
    public Guid HotelId { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
}
