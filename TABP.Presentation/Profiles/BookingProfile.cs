﻿using AutoMapper;
using TABP.Application.Bookings.Commands.Create;
using TABP.Presentation.DTOs.Booking;
namespace TABP.Presentation.Profiles;

public class BookingProfile : Profile
{
    public BookingProfile()
    {
        CreateMap<CreateBookingRequest, CreateBookingCommand>();
    }
}
