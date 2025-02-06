using AutoMapper;
using TABP.Application.Bookings.Add;
using TABP.Presentation.DTOs.Booking;

namespace TABP.Presentation.Profiles;
public class BookingProfile : Profile
{
    public BookingProfile()
    {
        CreateMap<AddBookingRequest, AddBookingCommand>();
    }
}
