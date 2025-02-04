using AutoMapper;
using TABP.Application.Amenities.Add;
using TABP.Domain.Entities;

namespace TABP.Application.Profiles;
public class AmenityProfile  :Profile
{
    public AmenityProfile()
    {
        CreateMap<AddAmenityCommand, Amenity>();
    }
}
