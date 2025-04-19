using AutoMapper;
using TABP.Application.Bookings.Common;
using TABP.Domain.Entities;

namespace TABP.Application.Profiles;
public class BookingProfile : Profile
{
    public BookingProfile()
    {
        CreateMap<Booking, BookingResponse>()
            .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Invoice.TotalPrice))
            .ForMember(dest => dest.RoomIds, opt => opt.MapFrom(src => src.Rooms.Select(r => r.Id)));
    }
}