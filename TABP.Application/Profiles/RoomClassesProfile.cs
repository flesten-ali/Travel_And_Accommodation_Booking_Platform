using AutoMapper;
using TABP.Application.RoomClasses.Commands.Create;
using TABP.Application.RoomClasses.Commands.ImageGallery;
using TABP.Application.RoomClasses.Commands.Update;
using TABP.Application.RoomClasses.Common;
using TABP.Application.RoomClasses.Queries.GetForAdmin;
using TABP.Application.RoomClasses.Queries.GetForHotel;
using TABP.Domain.Entities;
using TABP.Domain.Enums;
using TABP.Domain.Models;
namespace TABP.Application.Profiles;

public class RoomClassesProfile : Profile
{
    public RoomClassesProfile()
    {
        CreateMap<RoomClass, HotelRoomClassesQueryResponse>()
            .ForMember(dest => dest.GalleryUrls, opt => opt.MapFrom(src => src.Gallery.Select(g => g.ImageUrl)))
            .ForMember(dest => dest.DiscountResponses, opt => opt.MapFrom(src => src.Discounts))
            .ForMember(dest => dest.AmenityResponses, opt => opt.MapFrom(src => src.Amenities));

        CreateMap<Discount, DiscountResponse>();

        CreateMap<Amenity, AmenityResponse>();

        CreateMap<PaginatedResponse<RoomClass>, PaginatedResponse<HotelRoomClassesQueryResponse>>();

        CreateMap<PaginatedResponse<RoomClassForAdminResult>, PaginatedResponse<RoomClassForAdminResponse>>();

        CreateMap<RoomClassForAdminResult, RoomClassForAdminResponse>();

        CreateMap<CreateRoomClassCommand, RoomClass>();

        CreateMap<RoomClass, RoomClassResponse>();

        CreateMap<UpdateRoomClassCommand, RoomClass>();

        CreateMap<UploadRoomClassImageGalleryCommand, Image>()
            .ForMember(dest => dest.ImageType, opt => opt.MapFrom(src => ImageType.Gallery))
            .ForMember(dest => dest.ImageableId, opt => opt.MapFrom(src => src.RoomClassId));
    }
}
