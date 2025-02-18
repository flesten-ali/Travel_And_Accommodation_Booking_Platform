using MediatR;

namespace TABP.Application.CartItems.Commands.Delete;
public class DeleteCartItemCommand : IRequest
{
    public Guid Id { get; set; }
}
