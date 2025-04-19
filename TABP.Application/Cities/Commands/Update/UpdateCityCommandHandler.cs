using AutoMapper;
using MediatR;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Cities.Commands.Update;

/// <summary>
/// Handles the command to update the details of a city.
/// </summary>
public class UpdateCityCommandHandler : IRequestHandler<UpdateCityCommand>
{
    private readonly ICityRepository _cityRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateCityCommandHandler(ICityRepository cityRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _cityRepository = cityRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the request to update a city's details.
    /// </summary>
    /// <param name="request">The request containing the city details to be updated.</param>
    /// <param name="cancellationToken">The cancellation token for handling task cancellation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="NotFoundException">Thrown if the city with the specified ID does not exist.</exception>
    public async Task<Unit> Handle(UpdateCityCommand request, CancellationToken cancellationToken = default)
    {
        var city = await _cityRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(CityExceptionMessages.NotFound);

        _mapper.Map(request, city);

        _cityRepository.Update(city);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
