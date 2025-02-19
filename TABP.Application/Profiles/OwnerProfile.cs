using AutoMapper;
using TABP.Application.Owners.Commands.Create;
using TABP.Application.Owners.Common;
using TABP.Domain.Entities;

namespace TABP.Application.Profiles;
public class OwnerProfile : Profile
{
    public OwnerProfile()
    {
        CreateMap<CreateOwnerCommand, Owner>();

        CreateMap<Owner, OwnerResponse>();
    }
}
