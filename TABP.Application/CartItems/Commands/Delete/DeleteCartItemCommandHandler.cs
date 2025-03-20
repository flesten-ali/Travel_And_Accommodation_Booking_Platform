using MediatR;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.CartItems.Commands.Delete;

/// <summary>
/// Handles the command to delete a cart item.
/// </summary>
public class DeleteCartItemCommandHandler : IRequestHandler<DeleteCartItemCommand>
{
    private readonly ICartItemRepository _cartItemRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;

    public DeleteCartItemCommandHandler(
        ICartItemRepository cartItemRepository,
        IUnitOfWork unitOfWork,
        IUserRepository userRepository)
    {
        _cartItemRepository = cartItemRepository;
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
    }

    /// <summary>
    /// Handles the request to delete a cart item.
    /// </summary>
    /// <param name="request">The command containing the cart item ID and user ID.</param>
    /// <param name="cancellationToken">The cancellation token for handling task cancellation.</param>
    /// <returns>A task that represents the asynchronous operation, containing the result of the operation.</returns>
    /// <exception cref="NotFoundException">Thrown when the user or cart item does not exist.</exception>
    public async Task<Unit> Handle(DeleteCartItemCommand request, CancellationToken cancellationToken = default)
    {
        if (!await _userRepository.ExistsAsync(u => u.Id == request.UserId, cancellationToken))
        {
            throw new NotFoundException(UserExceptionMessages.NotFound);
        }

        if (!await _cartItemRepository.ExistsAsync(c => c.Id == request.CartItemId && c.UserId == request.UserId, cancellationToken))
        {
            throw new NotFoundException(CartItemExceptionMessages.NotFoundForUser);
        }

        _cartItemRepository.Delete(request.CartItemId);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
