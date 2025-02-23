using MediatR;
using TABP.Application.Shared;
using TABP.Domain.Models;

namespace TABP.Application.RoomClasses.Queries.GetForAdmin;
public sealed record GetRoomClassesForAdminQuery(PaginationParameters PaginationParameters) 
    : IRequest<PaginatedResponse<RoomClassForAdminResponse>>;
