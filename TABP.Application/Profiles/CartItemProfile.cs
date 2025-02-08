using AutoMapper;
using TABP.Application.CartItems.AddToCart;
using TABP.Domain.Entities;

namespace TABP.Application.Profiles;
public class CartItemProfile : Profile
{
    public CartItemProfile()
    {
        CreateMap<AddToCartCommand, CartItem>();
    }
}