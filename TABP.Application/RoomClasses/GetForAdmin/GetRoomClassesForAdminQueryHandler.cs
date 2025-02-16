using AutoMapper;
using MediatR;
using TABP.Application.Shared;
using TABP.Domain.Entities;
using TABP.Domain.Enums;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Models;

namespace TABP.Application.RoomClasses.GetForAdmin;
public class GetRoomClassesForAdminQueryHandler
    : IRequestHandler<GetRoomClassesForAdminQuery, PaginatedList<RoomClassForAdminResponse>>
{
    private readonly IRoomClassRepository _roomClassRepository;
    private readonly IMapper _mapper;

    public GetRoomClassesForAdminQueryHandler(IRoomClassRepository roomClassRepository, IMapper mapper)
    {
        _roomClassRepository = roomClassRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<RoomClassForAdminResponse>> Handle(
        GetRoomClassesForAdminQuery request,
        CancellationToken cancellationToken)
    {
        var orderBy = BuildSort(request.PaginationParameters);

        var roomClasses = await _roomClassRepository.GetRoomClassesForAdminAsync(
            orderBy,
            request.PaginationParameters.PageSize,
            request.PaginationParameters.PageNumber);

        return _mapper.Map<PaginatedList<RoomClassForAdminResponse>>(roomClasses);
    }

    private static Func<IQueryable<RoomClass>, IOrderedQueryable<RoomClass>> BuildSort(PaginationParameters paginationParameters)
    {
        var isDescending = paginationParameters.SortOrder == SortOrder.Descending;
        return paginationParameters.OrderColumn switch
        {
            "date" => isDescending
                    ? (roomClasses) => roomClasses.OrderByDescending(x => x.CreatedDate)
                    : (roomClasses) => roomClasses.OrderBy(x => x.CreatedDate),

            "adultscapacity" => isDescending
                    ? (roomClasses) => roomClasses.OrderByDescending(x => x.AdultsCapacity)
                    : (roomClasses) => roomClasses.OrderBy(x => x.AdultsCapacity),

            "childrencapacity" => isDescending
          ? (roomClasses) => roomClasses.OrderByDescending(x => x.ChildrenCapacity)
          : (roomClasses) => roomClasses.OrderBy(x => x.ChildrenCapacity),

            _ => (roomClasses) => roomClasses.OrderBy(h => h.Id)
        };
    }
}