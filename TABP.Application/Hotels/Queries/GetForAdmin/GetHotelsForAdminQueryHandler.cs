using AutoMapper;
using MediatR;
using TABP.Domain.Entities;
using TABP.Domain.Enums;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Models;

namespace TABP.Application.Hotels.Queries.GetForAdmin;
public class GetHotelsForAdminQueryHandler
    : IRequestHandler<GetHotelsForAdminQuery, PaginatedList<HotelForAdminResponse>>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IMapper _mapper;

    public GetHotelsForAdminQueryHandler(IHotelRepository hotelRepository, IMapper mapper)
    {
        _hotelRepository = hotelRepository;
        _mapper = mapper;
    }
    public async Task<PaginatedList<HotelForAdminResponse>> Handle(GetHotelsForAdminQuery request, CancellationToken cancellationToken)
    {
        var orderBy = BuildSort(request.PaginationParameters);
        var hotels = await _hotelRepository.GetHotelsForAdminAsync(
            orderBy,
            request.PaginationParameters.PageSize,
            request.PaginationParameters.PageNumber);

        return _mapper.Map<PaginatedList<HotelForAdminResponse>>(hotels);
    }

    private static Func<IQueryable<Hotel>, IOrderedQueryable<Hotel>> BuildSort(PaginationParameters paginationParameters)
    {
        var isDescending = paginationParameters.SortOrder == SortOrder.Descending;
        return paginationParameters.OrderColumn?.ToLower().Trim() switch
        {
            "name" => isDescending
            ? hotels => hotels.OrderByDescending(x => x.Name)
            : hotels => hotels.OrderBy(x => x.Name),

            "starrating" => isDescending
            ? hotels => hotels.OrderByDescending(x => x.Rate)
            : hotels => hotels.OrderBy(x => x.Rate),

            _ => hotels => hotels.OrderBy(x => x.Id),
        };
    }
}