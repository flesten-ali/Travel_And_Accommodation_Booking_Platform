using MediatR;
using TABP.Application.Reviews.Common;

namespace TABP.Application.Reviews.Queries.GetById;
public sealed record GetReviewByIdQuery(Guid ReviewId, Guid HotelId) : IRequest<ReviewResponse>;
