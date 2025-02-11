using AutoMapper;
using MediatR;
using TABP.Application.Amenities.Common;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Amenities.Queries.GetById;
internal class GetAmenityByIdQuerHandler : IRequestHandler<GetAmenityByIdQuery, AmenityResponse>
{
    private readonly IAmenityRepository _amenityRepository;
    private readonly IMapper _mapper;

    public GetAmenityByIdQuerHandler(IAmenityRepository amenityRepository, IMapper mapper)
    {
        _amenityRepository = amenityRepository;
        _mapper = mapper;
    }

    public async Task<AmenityResponse> Handle(GetAmenityByIdQuery request, CancellationToken cancellationToken)
    {
        var amenity = await _amenityRepository.GetByIdAsync(request.AmenityId)
            ?? throw new NotFoundException("Amenity not found");

        return _mapper.Map<AmenityResponse>(amenity);
    }
}
