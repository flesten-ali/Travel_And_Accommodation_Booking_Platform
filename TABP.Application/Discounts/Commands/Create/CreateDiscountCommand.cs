using MediatR;
using TABP.Application.Discounts.Common;

namespace TABP.Application.Discounts.Commands.Create;
public class CreateDiscountCommand : IRequest<DiscountResponse>
{
    public double Percentage { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid RoomClassId { get; set; }
}
