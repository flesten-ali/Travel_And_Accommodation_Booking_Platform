using TABP.Application.Shared;

namespace TABP.Presentation.DTOs.CartItem;
public class GetCartItemsRequest
{
    public Guid UserId { get; set; }
    public PaginationParameters PaginationParameters { get; set; }
}
