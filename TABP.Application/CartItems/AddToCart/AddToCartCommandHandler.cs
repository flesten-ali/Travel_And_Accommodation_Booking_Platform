using AutoMapper;
using MediatR;
using TABP.Application.Exceptions;
using TABP.Application.Exceptions.Messages;
using TABP.Domain.Entities;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.CartItems.AddToCart;
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
    public async Task<Unit> Handle(AddToCartCommand request, CancellationToken cancellationToken)
    {
        _ = await _userRepository.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException(UserExceptionMessages.NotFound);

        _ = await _roomClassRepository.GetByIdAsync(request.RoomClassId)
            ?? throw new NotFoundException(RoomClassExceptionMessages.NotFound);

        var cartItem = _mapper.Map<CartItem>(request);

        await _cartItemRepository.CreateAsync(cartItem);
        await _unitOfWork.SaveChangesAsync();

        return Unit.Value;
    }
}