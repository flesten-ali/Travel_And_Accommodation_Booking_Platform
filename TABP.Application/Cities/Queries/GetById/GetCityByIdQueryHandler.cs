using AutoMapper;
using MediatR;
using TABP.Application.Cities.Common;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Cities.Queries.GetById;

/// <summary>
/// Handles the query to retrieve a city by its ID.
/// </summary>
public class GetCityByIdQueryHandler : IRequestHandler<GetCityByIdQuery, CityResponse>
{
    private readonly ICityRepository _cityRepository;
    private readonly IMapper _mapper;

    public GetCityByIdQueryHandler(ICityRepository cityRepository, IMapper mapper)
    {
        _cityRepository = cityRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the request to retrieve a city by its ID.
    /// </summary>
    /// <param name="request">The request containing the city ID to be retrieved.</param>
    /// <param name="cancellationToken">The cancellation token for handling task cancellation.</param>
    /// <returns>A task that represents the asynchronous operation, containing the <see cref="CityResponse"/>.</returns>
    /// <exception cref="NotFoundException">Thrown if the city with the specified ID is not found.</exception>
    public async Task<CityResponse> Handle(GetCityByIdQuery request, CancellationToken cancellationToken = default)
    {
        var city = await _cityRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(CityExceptionMessages.NotFound);

        return _mapper.Map<CityResponse>(city);
    }
}
