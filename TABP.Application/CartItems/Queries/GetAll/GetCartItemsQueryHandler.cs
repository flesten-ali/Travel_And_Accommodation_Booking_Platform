using AutoMapper;
using MediatR;
using TABP.Application.Helpers;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Models;

namespace TABP.Application.CartItems.Queries.GetAll;

/// <summary>
/// Handles the query to retrieve all cart items for a user with pagination.
/// </summary>
public class GetCartItemsQueryHandler : IRequestHandler<GetCartItemsQuery, PaginatedResponse<CartItemResponse>>
{
    private readonly ICartItemRepository _cartItemRepository;
    private readonly IMapper _mapper;

    public GetCartItemsQueryHandler(ICartItemRepository cartItemRepository, IMapper mapper)
    {
        _cartItemRepository = cartItemRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the request to get all cart items for a user with pagination.
    /// </summary>
    /// <param name="request">The query containing pagination and user details.</param>
    /// <param name="cancellationToken">The cancellation token for handling task cancellation.</param>
    /// <returns>A task that represents the asynchronous operation, containing a paginated response with cart items.</returns>
    /// <exception cref="BadRequestException">Thrown when the cart is empty or doesn't exist.</exception>
    public async Task<PaginatedResponse<CartItemResponse>> Handle(GetCartItemsQuery request, CancellationToken cancellationToken = default)
    {
        if (!await _cartItemRepository.ExistsAsync(c => c.UserId == request.UserId, cancellationToken))
        {
            throw new BadRequestException(CartExceptionMessages.CartEmpty);
        }

        var orderBy = SortBuilder.BuildCartItemSort(request.PaginationParameters);

        var cartItems = await _cartItemRepository.GetCartItemsAsync(
            orderBy,
            request.PaginationParameters.PageSize,
            request.PaginationParameters.PageNumber,
            cancellationToken);

        return _mapper.Map<PaginatedResponse<CartItemResponse>>(cartItems);
    }
}
