﻿using AutoMapper;
using TABP.Application.Amenities.Create;
using TABP.Presentation.DTOs.Amenity;
namespace TABP.Presentation.Profiles;

public class AmenityProfile : Profile
{
    public AmenityProfile()
    {

        CreateMap<CreateAmenityRequest, CreateAmenityCommand>();
    }
}
