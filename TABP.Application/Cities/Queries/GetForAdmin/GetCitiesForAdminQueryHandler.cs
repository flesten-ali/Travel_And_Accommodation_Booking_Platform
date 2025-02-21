using AutoMapper;
using MediatR;
using TABP.Application.Helpers;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Models;

namespace TABP.Application.Cities.Queries.GetForAdmin;
public class GetCitiesForAdminQueryHandler
    : IRequestHandler<GetCitiesForAdminQuery, PaginatedResponse<CityForAdminResponse>>
{
    private readonly ICityRepository _cityRepository;
    private readonly IMapper _mapper;

    public GetCitiesForAdminQueryHandler(ICityRepository cityRepository, IMapper mapper)
    {
        _cityRepository = cityRepository;
        _mapper = mapper;
    }
    public async Task<PaginatedResponse<CityForAdminResponse>> Handle(
        GetCitiesForAdminQuery request,
        CancellationToken cancellationToken = default)
    {
        var orderBy = SortBuilder.BuildCitySort(request.PaginationParameters);

        var cities = await _cityRepository
            .GetCitiesForAdminAsync(
            orderBy,
            request.PaginationParameters.PageSize,
            request.PaginationParameters.PageNumber,
            cancellationToken);

        return _mapper.Map<PaginatedResponse<CityForAdminResponse>>(cities);
    }
}