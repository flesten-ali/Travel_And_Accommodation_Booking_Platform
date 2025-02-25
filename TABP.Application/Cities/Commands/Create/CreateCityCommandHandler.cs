using AutoMapper;
using MediatR;
using TABP.Application.Cities.Common;
using TABP.Domain.Entities;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Cities.Commands.Create;

/// <summary>
/// Handles the command to create a new city.
/// </summar
public class CreateCityCommandHandler : IRequestHandler<CreateCityCommand, CityResponse>
{
    private readonly ICityRepository _cityRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCityCommandHandler(ICityRepository cityRepository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _cityRepository = cityRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Handles the request to create a new city.
    /// </summary>
    /// <param name="request">The request containing city data.</param>
    /// <param name="cancellationToken">The cancellation token for handling task cancellation.</param>
    /// <returns>A task that represents the asynchronous operation, containing the created city response.</returns>
    public async Task<CityResponse> Handle(CreateCityCommand request, CancellationToken cancellationToken = default)
    {
        var city = _mapper.Map<City>(request);

        await _cityRepository.CreateAsync(city, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CityResponse>(city);
    }
}
