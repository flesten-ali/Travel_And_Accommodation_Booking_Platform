using AutoMapper;
using TABP.Application.RoomClasses.Queries.GetDetails;
using TABP.Domain.Entities;
using TABP.Domain.Models;
namespace TABP.Application.Profiles;

public class RoomClassesProfile : Profile
{
    public RoomClassesProfile()
    {
        CreateMap<RoomClass, GetRoomClassDetailsQueryResponse>()
            .ForMember(dest => dest.GalleryUrls, opt => opt.MapFrom(src => src.Gallery.Select(g => g.ImageUrl)))
            .ForMember(dest => dest.DiscountResponses, opt => opt.MapFrom(src => src.Discounts))
            .ForMember(dest => dest.AmenityResponses, opt => opt.MapFrom(src => src.Amenities));

        CreateMap<Discount, DiscountResponse>();

        CreateMap<Amenity, AmenityResponse>();

        CreateMap<PaginatedList<RoomClass>, PaginatedList<GetRoomClassDetailsQueryResponse>>();
    }
}
