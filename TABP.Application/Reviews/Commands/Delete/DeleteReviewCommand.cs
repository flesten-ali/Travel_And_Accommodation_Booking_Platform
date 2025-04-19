using MediatR;

namespace TABP.Application.Reviews.Commands.Delete;
public sealed record DeleteReviewCommand(Guid ReviewId, Guid UserId, Guid HotelId) : IRequest;
