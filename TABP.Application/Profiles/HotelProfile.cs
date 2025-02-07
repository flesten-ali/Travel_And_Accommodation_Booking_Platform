using AutoMapper;
using TABP.Application.Hotels.Commands.AddHotel;
using TABP.Application.Hotels.Commands.AddImageGallery;
using TABP.Application.Hotels.Commands.AddThumbnail;
using TABP.Application.Hotels.Queries.GetHotelDetails;
using TABP.Application.Hotels.Queries.Search;
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
            .ForMember(dest => dest.ImageableType, opt => opt.MapFrom(src => ImageableType.Hotel))
            .ForMember(dest => dest.ImageType, opt => opt.MapFrom(src => ImageType.Thumbnail));

        CreateMap<AddImageGalleryCommand, Image>()
        .ForMember(dest => dest.ImageableId, opt => opt.MapFrom(src => src.HotelId))
        .ForMember(dest => dest.ImageableType, opt => opt.MapFrom(src => ImageableType.Hotel))
        .ForMember(dest => dest.ImageType, opt => opt.MapFrom(src => ImageType.Gallery));


        CreateMap<Hotel, GetHotelDetailsResponse>()
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City.Name))
            .ForMember(dest => dest.GalleryUrls, opt => opt.MapFrom(src => src.Gallery.Select(g => g.ImageUrl)));
    }
}
