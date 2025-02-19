using MediatR;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.CartItems.Commands.Delete;
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

    public async Task<Unit> Handle(DeleteCartItemCommand request, CancellationToken cancellationToken = default)
    {
        if (!await _userRepository.ExistsAsync(u => u.Id == request.UserId, cancellationToken))
        {
            throw new NotFoundException(UserExceptionMessages.NotFound);
        }

        if (!await _cartItemRepository.ExistsAsync(c => c.Id == request.CartId && c.UserId == request.UserId, cancellationToken))
        {
            throw new NotFoundException(CartItemExceptionMessages.NotFoundForUser);
        }

        _cartItemRepository.Delete(request.CartId);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
