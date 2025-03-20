using AutoMapper;
using MediatR;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Entities;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.CartItems.Commands.Create;

/// <summary>
/// Handles the command to create a new cart item.
/// </summary>
public class CreateCartItemCommandHandler : IRequestHandler<CreateCartItemCommand>
{
    private readonly ICartItemRepository _cartItemRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRoomClassRepository _roomClassRepository;
    private readonly IUserRepository _userRepository;

    public CreateCartItemCommandHandler(
        ICartItemRepository cartItemRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IRoomClassRepository roomClassRepository,
        IUserRepository userRepository)
    {
        _cartItemRepository = cartItemRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _roomClassRepository = roomClassRepository;
        _userRepository = userRepository;
    }

    /// <summary>
    /// Handles the request to create a new cart item.
    /// </summary>
    /// <param name="request">The command containing the cart item details.</param>
    /// <param name="cancellationToken">The cancellation token for handling task cancellation.</param>
    /// <returns>A task that represents the asynchronous operation, containing the result of the operation.</returns>
    /// <exception cref="NotFoundException">Thrown when the user or room class does not exist.</exception>
    public async Task<Unit> Handle(CreateCartItemCommand request, CancellationToken cancellationToken = default)
    {
        if (!await _userRepository.ExistsAsync(x => x.Id == request.UserId, cancellationToken))
        {
            throw new NotFoundException(UserExceptionMessages.NotFound);
        }

        if (!await _roomClassRepository.ExistsAsync(x => x.Id == request.RoomClassId, cancellationToken))
        {
            throw new NotFoundException(RoomClassExceptionMessages.NotFound);
        }

        var cartItem = _mapper.Map<CartItem>(request);

        await _cartItemRepository.CreateAsync(cartItem, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}