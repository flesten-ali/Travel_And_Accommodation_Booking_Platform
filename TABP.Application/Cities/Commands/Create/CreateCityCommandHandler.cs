using AutoMapper;
using MediatR;
using TABP.Application.Cities.Common;
using TABP.Domain.Entities;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Cities.Commands.Create;
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

    public async Task<CityResponse> Handle(CreateCityCommand request, CancellationToken cancellationToken)
    {
        var city = _mapper.Map<City>(request);

        await _cityRepository.AddAsync(city);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CityResponse>(city);
    }
}
