using AutoMapper;
using TABP.Application.Amenities.Commands.Create;
using TABP.Application.Amenities.Common;
using TABP.Domain.Entities;

namespace TABP.Application.Profiles;
public class AmenityProfile : Profile
{
    public AmenityProfile()
    {
        CreateMap<CreateAmenityCommand, Amenity>();
        CreateMap<Amenity, AmenityResponse>();
    }
}
