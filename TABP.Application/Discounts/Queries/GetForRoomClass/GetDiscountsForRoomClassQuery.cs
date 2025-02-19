using MediatR;
using TABP.Application.Discounts.Common;
using TABP.Application.Shared;
using TABP.Domain.Models;

namespace TABP.Application.Discounts.Queries.GetForRoomClass;
public class GetDiscountsForRoomClassQuery : IRequest<PaginatedList<DiscountResponse>>
{
    public Guid RoomClassId { get; set; }
    public PaginationParameters PaginationParameters { get; set; }
}