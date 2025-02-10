using AutoMapper;
using TABP.Application.Hotels.Commands.Create;
using TABP.Application.Hotels.Commands.ImageGallery;
using TABP.Application.Hotels.Commands.Thumbnail;
using TABP.Application.Hotels.Common;
using TABP.Application.Hotels.Queries.GetDetails;
using TABP.Application.Hotels.Queries.GetFeaturedDeals;
using TABP.Application.Hotels.Queries.GetRecentlyVisited;
using TABP.Application.Hotels.Queries.SearchHotels;
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

        CreateMap<CreateHotelCommand, Hotel>();

        CreateMap<UploadThumbnailCommand, Image>()
            .ForMember(dest => dest.ImageableId, opt => opt.MapFrom(src => src.HotelId))
            .ForMember(dest => dest.ImageType, opt => opt.MapFrom(src => ImageType.Thumbnail));

        CreateMap<UploadImageGalleryCommand, Image>()
        .ForMember(dest => dest.ImageableId, opt => opt.MapFrom(src => src.HotelId))
        .ForMember(dest => dest.ImageType, opt => opt.MapFrom(src => ImageType.Gallery));


        CreateMap<Hotel, HotelDetailsResponse>()
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City.Name))
            .ForMember(dest => dest.GalleryUrls, opt => opt.MapFrom(src => src.Gallery.Select(g => g.ImageUrl)));

        CreateMap<Hotel, HotelResponse>()
            .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner.Name))
            .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City.Name))
            .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom(src => src.Thumbnail.ImageUrl));

        CreateMap<FeaturedDealResult, FeaturedDealResponse>();

        CreateMap<RecentlyVisitedHotelsResult, RecentlyVisitedHotelsResponse>();
    }
}