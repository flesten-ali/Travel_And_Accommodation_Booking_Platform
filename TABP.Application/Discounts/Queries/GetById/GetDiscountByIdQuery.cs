using MediatR;
using TABP.Application.Discounts.Common;

namespace TABP.Application.Discounts.Queries.GetById;
public sealed record GetDiscountByIdQuery(Guid DiscountId, Guid RoomClassId) : IRequest<DiscountResponse>;
