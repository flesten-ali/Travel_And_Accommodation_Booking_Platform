using MediatR;

namespace TABP.Application.CartItems.Commands.Delete;
public sealed record DeleteCartItemCommand(Guid CartId, Guid UserId) : IRequest;
