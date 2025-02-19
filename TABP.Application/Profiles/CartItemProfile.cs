using AutoMapper;
using TABP.Application.CartItems.Commands.Create;
using TABP.Application.CartItems.Queries.GetAll;
using TABP.Domain.Entities;
using TABP.Domain.Models;

namespace TABP.Application.Profiles;
public class CartItemProfile : Profile
{
    public CartItemProfile()
    {
        CreateMap<CreateCartItemCommand, CartItem>();

        CreateMap<CartItem, CartItemResponse>();

        CreateMap<PaginatedList<CartItem>, PaginatedList<CartItemResponse>>();
    }
}