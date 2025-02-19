using MediatR;
using TABP.Application.Discounts.Common;

namespace TABP.Application.Discounts.Queries.GetById;
public class GetDiscountByIdQuery : IRequest<DiscountResponse>
{
    public Guid DiscountId { get; set; }
    public Guid RoomClassId { get; set; }
}
