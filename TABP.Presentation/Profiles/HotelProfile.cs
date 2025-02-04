using AutoMapper;
using TABP.Application.Hotels.Search;
using TABP.Presentation.DTOs.Hotel;
namespace TABP.Presentation.Profiles;

public class HotelProfile : Profile
{
    public HotelProfile()
    {
        CreateMap<SearchHotelRequest, SearchHotelCommand>();
    }
}
