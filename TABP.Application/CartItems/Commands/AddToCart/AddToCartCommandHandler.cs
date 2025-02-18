using AutoMapper;
using MediatR;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Entities;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.CartItems.Commands.AddToCart;
public class AddToCartCommandHandler : IRequestHandler<AddToCartCommand>
{
    private readonly ICartItemRepository _cartItemRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRoomClassRepository _roomClassRepository;
    private readonly IUserRepository _userRepository;

    public AddToCartCommandHandler(
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

    public async Task<Unit> Handle(AddToCartCommand request, CancellationToken cancellationToken = default)
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