using AutoMapper;
using MediatR;
using TABP.Application.Helpers;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Models;

namespace TABP.Application.Cities.Queries.GetForAdmin;

/// <summary>
/// Handles the query to retrieve cities for admin purposes with pagination and sorting.
/// </summary>
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

    /// <summary>
    /// Handles the request to retrieve a paginated list of cities for admin purposes.
    /// </summary>
    /// <param name="request">The request containing pagination and sorting parameters.</param>
    /// <param name="cancellationToken">The cancellation token for handling task cancellation.</param>
    /// <returns>
    /// A task that represents the asynchronous operation, containing a paginated response of <see cref="CityForAdminResponse"/>.
    /// </returns>
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