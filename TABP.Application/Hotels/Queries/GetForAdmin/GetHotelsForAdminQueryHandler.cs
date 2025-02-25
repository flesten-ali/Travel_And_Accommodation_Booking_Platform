using AutoMapper;
using MediatR;
using TABP.Application.Helpers;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Models;

namespace TABP.Application.Hotels.Queries.GetForAdmin;

/// <summary>
/// Handles the query to retrieve a paginated list of hotels for admin purposes.
/// </summary>
public class GetHotelsForAdminQueryHandler
    : IRequestHandler<GetHotelsForAdminQuery, PaginatedResponse<HotelForAdminResponse>>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IMapper _mapper;

    public GetHotelsForAdminQueryHandler(IHotelRepository hotelRepository, IMapper mapper)
    {
        _hotelRepository = hotelRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the query to retrieve a paginated list of hotels for the admin.
    /// </summary>
    /// <param name="request">The query containing the pagination parameters.</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the operation if needed.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The result is a paginated list of <see cref="HotelForAdminResponse"/>.
    /// </returns>
    public async Task<PaginatedResponse<HotelForAdminResponse>> Handle(
        GetHotelsForAdminQuery request,
        CancellationToken cancellationToken = default)
    {
        var orderBy = SortBuilder.BuildHotelSort(request.PaginationParameters);
        var hotels = await _hotelRepository.GetHotelsForAdminAsync(
            orderBy,
            request.PaginationParameters.PageSize,
            request.PaginationParameters.PageNumber,
            cancellationToken);

        return _mapper.Map<PaginatedResponse<HotelForAdminResponse>>(hotels);
    }
}