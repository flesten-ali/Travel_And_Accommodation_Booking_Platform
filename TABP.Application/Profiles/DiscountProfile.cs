using AutoMapper;
using TABP.Application.Discounts.Commands.Create;
using TABP.Application.Discounts.Common;
using TABP.Domain.Entities;
using TABP.Domain.Models;

namespace TABP.Application.Profiles;
public class DiscountProfile : Profile
{
    public DiscountProfile()
    {
        CreateMap<CreateDiscountCommand, Discount>();

        CreateMap<Discount, DiscountResponse>();

        CreateMap<PaginatedList<Discount>, PaginatedList<DiscountResponse>>();
    }
}
