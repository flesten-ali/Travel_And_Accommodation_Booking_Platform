using AutoMapper;
using MediatR;
using TABP.Application.Amenities.Common;
using TABP.Application.Exceptions;
using TABP.Application.Exceptions.Messages;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Amenities.Queries.GetById;
public class GetAmenityByIdQuerHandler : IRequestHandler<GetAmenityByIdQuery, AmenityResponse>
{
    private readonly IAmenityRepository _amenityRepository;
    private readonly IMapper _mapper;

    public GetAmenityByIdQuerHandler(IAmenityRepository amenityRepository, IMapper mapper)
    {
        _amenityRepository = amenityRepository;
        _mapper = mapper;
    }

    public async Task<AmenityResponse> Handle(GetAmenityByIdQuery request, CancellationToken cancellationToken = default)
    {
        var amenity = await _amenityRepository.GetByIdAsync(request.AmenityId, cancellationToken)
            ?? throw new NotFoundException(AmenityExceptionMessages.NotFound);

        return _mapper.Map<AmenityResponse>(amenity);
    }
}
