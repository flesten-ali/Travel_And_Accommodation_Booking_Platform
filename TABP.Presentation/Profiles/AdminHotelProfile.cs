using AutoMapper;
using TABP.Application.Hotels.Commands.AddHotel;
using TABP.Application.Hotels.Commands.AddImageGallery;
using TABP.Application.Hotels.Commands.AddThumbnail;
using TABP.Presentation.DTOs;
using TABP.Presentation.DTOs.Hotel;
namespace TABP.Presentation.Profiles;

public class AdminHotelProfile : Profile
{
    public AdminHotelProfile()
    {
        CreateMap<AddHotelRequest, AddHotelCommand>();
        CreateMap<AddThumbnailRequest, AddThumbnailCommand>();
        CreateMap<AddImageGalleryRequest, AddImageGalleryCommand>();
    }
}
