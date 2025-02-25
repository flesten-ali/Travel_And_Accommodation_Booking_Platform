using MediatR;

namespace TABP.Application.CartItems.Commands.Delete;
public sealed record DeleteCartItemCommand(Guid CartItemId, Guid UserId) : IRequest;
