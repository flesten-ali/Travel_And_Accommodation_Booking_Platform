using AutoMapper;
using TABP.Application.Amenities.Create;
using TABP.Domain.Entities;

namespace TABP.Application.Profiles;
public class AmenityProfile  :Profile
{
    public AmenityProfile()
    {
        CreateMap<CreateAmenityCommand, Amenity>();
    }
}
