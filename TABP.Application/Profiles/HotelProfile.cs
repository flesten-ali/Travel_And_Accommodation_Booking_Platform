using AutoMapper;
using TABP.Application.Hotels.Add;
using TABP.Application.Hotels.AddImageGallery;
using TABP.Application.Hotels.AddThumbnail;
using TABP.Application.Hotels.Search;
using TABP.Domain.Entities;
using TABP.Domain.Enums;
using TABP.Domain.Models;
namespace TABP.Application.Profiles;

public class HotelProfile : Profile
{
    public HotelProfile()
    {
        CreateMap<SearchHotelResult, SearchHotelResponse>();

        CreateMap<PaginatedList<SearchHotelResult>, PaginatedList<SearchHotelResponse>>();

        CreateMap<AddHotelCommand, Hotel>();

        CreateMap<AddThumbnailCommand, Image>()
            .ForMember(dest => dest.ImageableId, opt => opt.MapFrom(src => src.HotelId))
            .ForMember(dest => dest.ImageableType, opt => opt.MapFrom(src => ImageableType.Hotel));

        CreateMap<AddImageGalleryCommand, Image>()
        .ForMember(dest => dest.ImageableId, opt => opt.MapFrom(src => src.HotelId))
        .ForMember(dest => dest.ImageableType, opt => opt.MapFrom(src => ImageableType.Hotel));
    }
}
