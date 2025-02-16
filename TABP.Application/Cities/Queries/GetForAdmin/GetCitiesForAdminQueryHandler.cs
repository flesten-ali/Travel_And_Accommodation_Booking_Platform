using AutoMapper;
using MediatR;
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
        var orderBy = BuildSort(request.PaginationParameters);

        var cities = await _cityRepository
            .GetCitiesForAdminAsync(
            request.PaginationParameters.PageSize,
            request.PaginationParameters.PageNumber,
            orderBy);

        return _mapper.Map<PaginatedList<CityForAdminResponse>>(cities);
    }

    private static Func<IQueryable<City>, IOrderedQueryable<City>> BuildSort(PaginationParameters paginationParameters)
    {
        var isDescending = paginationParameters.SortOrder == SortOrder.Descending;
        return paginationParameters.OrderColumn?.ToLower().Trim() switch
        {
            "name" => isDescending
            ? cities => cities.OrderByDescending(x => x.Name)
            : cities => cities.OrderBy(x => x.Name),

            "country" => isDescending
            ? cities => cities.OrderByDescending(x => x.Country)
            : cities => cities.OrderBy(x => x.Country),

            _ => cities => cities.OrderBy(x => x.Id),
        };
    }
}