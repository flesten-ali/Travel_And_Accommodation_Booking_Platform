using MediatR;

namespace TABP.Application.Hotels.Queries.GetRecentlyVisited;
public class GetRecentlyVisitedHotelsQuery :IRequest<IEnumerable<RecentlyVisitedHotelsResponse>>
{
    public Guid GuestId { get; set; }
    public int Limit { get; set; }
}