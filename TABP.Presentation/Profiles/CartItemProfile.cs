using AutoMapper;
using TABP.Application.CartItems.Commands.AddToCart;
using TABP.Application.CartItems.Queries.GetAll;
using TABP.Presentation.DTOs.CartItem;

namespace TABP.Presentation.Profiles;
public class CartItemProfile :Profile
{
    public CartItemProfile()
    {
        CreateMap<AddToCartRequest, AddToCartCommand>();

        CreateMap<GetCartItemsRequest, GetCartItemsQuery>();
    }
}
