using AutoMapper;
using MediatR;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Models;

namespace TABP.Application.Cities.Queries.GetForAdmin;
public class GetCitiesForAdminQueryHandler
    : IRequestHandler<GetCitiesForAdminQuery, PaginatedList<CityForAdminResponse>>
{
    private readonly ICityRepository _cityRepository;
    private readonly IMapper _mapper;

    public GetCitiesForAdminQueryHandler(ICityRepository cityRepository, IMapper mapper)
    {
        _cityRepository = cityRepository;
        _mapper = mapper;
    }
    public async Task<PaginatedList<CityForAdminResponse>> Handle(
        GetCitiesForAdminQuery request,
        CancellationToken cancellationToken)
    {
        var cities = await _cityRepository.GetCitiesForAdmin(request.PageSize, request.PageNumber);

        return _mapper.Map<PaginatedList<CityForAdminResponse>>(cities);
    }
}