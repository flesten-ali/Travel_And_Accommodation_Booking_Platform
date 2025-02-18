using MediatR;

namespace TABP.Application.CartItems.Delete;
public class DeleteCartItemCommand : IRequest
{
    public Guid Id { get; set; }
}
