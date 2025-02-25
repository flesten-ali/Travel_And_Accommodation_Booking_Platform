using AutoMapper;
using MediatR;
using TABP.Application.Amenities.Common;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Amenities.Queries.GetById;

/// <summary>
/// Handles retrieving an amenity by its ID.
/// Implements <see cref="IRequestHandler{TRequest, TResponse}"/> to process <see cref="GetAmenityByIdQuery"/> requests.
/// </summary>
public class GetAmenityByIdQuerHandler : IRequestHandler<GetAmenityByIdQuery, AmenityResponse>
{
    private readonly IAmenityRepository _amenityRepository;
    private readonly IMapper _mapper;

    public GetAmenityByIdQuerHandler(IAmenityRepository amenityRepository, IMapper mapper)
    {
        _amenityRepository = amenityRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the retrieval of an amenity by its ID.
    /// </summary>
    /// <param name="request">The <see cref="GetAmenityByIdQuery"/> containing the ID of the requested amenity.</param>
    /// <param name="cancellationToken">A cancellation token to observe while awaiting the asynchronous operation.</param>
    /// <returns>
    /// A task representing the asynchronous operation, returning an <see cref="AmenityResponse"/> with the amenity details.
    /// </returns>
    /// <exception cref="NotFoundException">
    /// Thrown when the amenity with the specified ID is not found.
    /// </exception>
    public async Task<AmenityResponse> Handle(GetAmenityByIdQuery request, CancellationToken cancellationToken = default)
    {
        var amenity = await _amenityRepository.GetByIdAsync(request.AmenityId, cancellationToken)
            ?? throw new NotFoundException(AmenityExceptionMessages.NotFound);

        return _mapper.Map<AmenityResponse>(amenity);
    }
}
