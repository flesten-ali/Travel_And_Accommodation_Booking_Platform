using MediatR;

namespace TABP.Application.Reviews.Commands.Update;
public class UpdateReviewCommand : IRequest
{
    public Guid Id { get; set; }
    public string Comment { get; set; }
    public int Rate { get; set; }
    public Guid HotelId { get; set; }
    public Guid UserId { get; set; }
}
