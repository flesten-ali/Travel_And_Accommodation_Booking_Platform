using MediatR;
using TABP.Application.Reviews.Common;

namespace TABP.Application.Reviews.Queries.GetById;
public class GetReviewByIdQuery : IRequest <ReviewResponse>
{
    public Guid HotelId { get; set; }
    public Guid ReviewId { get; set; }
}
