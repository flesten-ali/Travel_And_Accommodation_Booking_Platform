using AutoMapper;
using TABP.Application.CartItems.Queries.GetAll;
using TABP.Presentation.DTOs.CartItem;

namespace TABP.Presentation.Profiles;
public class CartItemProfile : Profile
{
    public CartItemProfile()
    {
        CreateMap<GetCartItemsRequest, GetCartItemsQuery>();
    }
}
