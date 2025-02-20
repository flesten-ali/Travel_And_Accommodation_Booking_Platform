using MediatR;

namespace TABP.Application.Reviews.Commands.Delete;
public class DeleteReviewCommand : IRequest
{
    public Guid ReviewId { get; set; }
    public Guid UserId { get; set; }
    public Guid HotelId { get; set; }
}
