using AutoMapper;
using MediatR;
using TABP.Application.Cities.Common;
using TABP.Application.Exceptions;
using TABP.Application.Exceptions.Messages;
using TABP.Domain.Interfaces.Persistence.Repositories;
namespace TABP.Application.Cities.Queries.GetById;

public class GetCityByIdQueryHandler : IRequestHandler<GetCityByIdQuery, CityResponse>
{
    private readonly ICityRepository _cityRepository;
    private readonly IMapper _mapper;

    public GetCityByIdQueryHandler(ICityRepository cityRepository, IMapper mapper)
    {
        _cityRepository = cityRepository;
        _mapper = mapper;
    }

    public async Task<CityResponse> Handle(GetCityByIdQuery request, CancellationToken cancellationToken = default)
    {
        var city = await _cityRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(CityExceptionMessages.NotFound);

        return _mapper.Map<CityResponse>(city);
    }
}
