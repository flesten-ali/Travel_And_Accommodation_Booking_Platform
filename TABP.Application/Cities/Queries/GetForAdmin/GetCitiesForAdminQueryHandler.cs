using AutoMapper;
using MediatR;
using TABP.Application.Helper;
using TABP.Application.Shared;
using TABP.Domain.Entities;
using TABP.Domain.Enums;
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
        var orderBy =SortBuilder.BuildCitySort(request.PaginationParameters);

        var cities = await _cityRepository
            .GetCitiesForAdminAsync(
            request.PaginationParameters.PageSize,
            request.PaginationParameters.PageNumber,
            orderBy);

        return _mapper.Map<PaginatedList<CityForAdminResponse>>(cities);
    }
}