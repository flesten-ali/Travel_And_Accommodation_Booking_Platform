using AutoMapper;
using TABP.Application.Hotels.Queries.SearchHotels;
using TABP.Presentation.DTOs.Hotel;
namespace TABP.Presentation.Profiles;

public class HotelProfile : Profile
{
    public HotelProfile()
    {
        CreateMap<SearchHotelRequest, SearchHotelsQuery>();
    }
}
