using MediatR;
using TABP.Application.Shared;
using TABP.Domain.Models;

namespace TABP.Application.CartItems.Queries.GetAll;
public class GetCartItemsQuery : IRequest<PaginatedResponse<CartItemResponse>>
{
    public Guid UserId { get; set; } 
    public PaginationParameters PaginationParameters { get; set; }
}
