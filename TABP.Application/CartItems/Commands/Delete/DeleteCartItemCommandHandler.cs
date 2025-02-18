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

    public DeleteCartItemCommandHandler(ICartItemRepository cartItemRepository, IUnitOfWork unitOfWork)
    {
        _cartItemRepository = cartItemRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteCartItemCommand request, CancellationToken cancellationToken = default)
    {
        if (!await _cartItemRepository.ExistsAsync(c => c.Id == request.Id, cancellationToken))
        {
            throw new NotFoundException(CartItemExceptionMessages.NotFound);
        }

        _cartItemRepository.Delete(request.Id);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
