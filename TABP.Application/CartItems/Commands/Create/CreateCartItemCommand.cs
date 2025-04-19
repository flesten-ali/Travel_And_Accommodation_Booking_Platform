using MediatR;

namespace TABP.Application.CartItems.Commands.Create;
public class CreateCartItemCommand : IRequest
{
    public Guid UserId { get; set; }
    public Guid RoomClassId { get; set; }
}