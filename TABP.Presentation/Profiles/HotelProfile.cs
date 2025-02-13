using AutoMapper;
using TABP.Application.Hotels.Commands.Create;
using TABP.Application.Hotels.Commands.ImageGallery;
using TABP.Application.Hotels.Commands.Thumbnail;
using TABP.Application.Hotels.Queries.SearchHotels;
using TABP.Presentation.DTOs;
using TABP.Presentation.DTOs.Hotel;
namespace TABP.Presentation.Profiles;

public class HotelProfile : Profile
{
    public HotelProfile()
    {
        CreateMap<CreateHotelRequest, CreateHotelCommand>();
        CreateMap<UploadHotelThumbnailRequest, UploadHotelThumbnailCommand>();
        CreateMap<UploadImageGalleryRequest, UploadImageGalleryCommand>();
        CreateMap<SearchHotelRequest, SearchHotelsQuery>();
    }
}
