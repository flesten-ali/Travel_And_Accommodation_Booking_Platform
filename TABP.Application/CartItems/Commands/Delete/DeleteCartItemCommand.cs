using MediatR;

namespace TABP.Application.CartItems.Commands.Delete;
public class DeleteCartItemCommand : IRequest
{
    public Guid UserId{ get; set; }
    public Guid CartId { get; set; }
}
