using AutoMapper;
using MediatR;
using TABP.Application.Helper;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Models;

namespace TABP.Application.CartItems.Queries.GetAll;
public class GetCartItemsQueryHandler : IRequestHandler<GetCartItemsQuery, PaginatedList<CartItemResponse>>
{
    private readonly ICartItemRepository _cartItemRepository;
    private readonly IMapper _mapper;

    public GetCartItemsQueryHandler(ICartItemRepository cartItemRepository, IMapper mapper)
    {
        _cartItemRepository = cartItemRepository;
        _mapper = mapper;
    }
    public async Task<PaginatedList<CartItemResponse>> Handle(GetCartItemsQuery request, CancellationToken cancellationToken = default)
    {
        if (!await _cartItemRepository.ExistsAsync(c => c.UserId == request.UserId, cancellationToken))
        {
            throw new CartEmptyException(CartExceptionMessages.CartEmpty);
        }

        var orderBy = SortBuilder.BuildCartItemSort(request.PaginationParameters);

        var cartItems = await _cartItemRepository.GetCartItemsAsync(
            orderBy,
            request.PaginationParameters.PageSize,
            request.PaginationParameters.PageNumber,
            cancellationToken);

        return _mapper.Map<PaginatedList<CartItemResponse>>(cartItems);
    }
}
