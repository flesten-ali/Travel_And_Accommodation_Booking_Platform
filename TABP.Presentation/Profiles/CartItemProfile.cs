using AutoMapper;
using TABP.Application.CartItems.AddToCart;
using TABP.Presentation.DTOs.CartItem;

namespace TABP.Presentation.Profiles;
public class CartItemProfile :Profile
{
    public CartItemProfile()
    {
        CreateMap<AddToCartRequest, AddToCartCommand>();
    }
}
