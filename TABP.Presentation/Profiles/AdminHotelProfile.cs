using AutoMapper;
using TABP.Application.Hotels.Commands.AddHotel;
using TABP.Application.Hotels.Commands.ImageGallery;
using TABP.Application.Hotels.Commands.Thumbnail;
using TABP.Presentation.DTOs;
using TABP.Presentation.DTOs.Hotel;
namespace TABP.Presentation.Profiles;

public class AdminHotelProfile : Profile
{
    public AdminHotelProfile()
    {
        CreateMap<CreateHotelRequest, CreateHotelCommand>();
        CreateMap<UploadThumbnailRequest, UploadThumbnailCommand>();
        CreateMap<UploadImageGalleryRequest, UploadImageGalleryCommand>();
    }
}
