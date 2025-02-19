using MediatR;

namespace TABP.Application.Discounts.Commands.Delete;
public class DeleteDiscountCommand : IRequest
{
    public Guid DiscountId { get; set; }
    public Guid RoomClassId { get; set; }
}
