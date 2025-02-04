using AutoMapper;
using TABP.Application.Hotels.Add;
using TABP.Application.Hotels.AddThumbnail;
using TABP.Presentation.DTOs;
using TABP.Presentation.DTOs.Hotel;

namespace TABP.Presentation.Profiles;
public class AdminHotelProfile : Profile
{
    public AdminHotelProfile()
    {
        CreateMap<AddHotelRequest, AddHotelCommand>();
        CreateMap<AddThumbnailRequest, AddThumbnailCommand>();
    }
}
