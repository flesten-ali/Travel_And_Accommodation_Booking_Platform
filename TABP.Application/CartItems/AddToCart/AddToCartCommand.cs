using MediatR;

namespace TABP.Application.CartItems.AddToCart;
public class AddToCartCommand : IRequest<Guid>
{
    public Guid UserId { get; set; }
    public Guid RoomClassId { get; set; }
}