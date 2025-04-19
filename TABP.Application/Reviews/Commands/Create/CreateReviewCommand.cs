using MediatR;
using TABP.Application.Reviews.Common;

namespace TABP.Application.Reviews.Commands.Create;
public class CreateReviewCommand : IRequest<ReviewResponse>
{
    public string Comment { get; set; }
    public int Rate { get; set; }
    public Guid HotelId { get; set; }
    public Guid UserId { get; set; }
}
