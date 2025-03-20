using AutoMapper;
using TABP.Application.Discounts.Commands.Create;
using TABP.Application.Discounts.Queries.GetForRoomClass;
using TABP.Presentation.DTOs.Discount;

namespace TABP.Presentation.Profiles;
public class DiscountProfile : Profile
{
    public DiscountProfile()
    {
        CreateMap<CreateDiscountRequest, CreateDiscountCommand>();

        CreateMap<GetDiscountsForRoomClassRequest, GetDiscountsForRoomClassQuery>();
    }
}
