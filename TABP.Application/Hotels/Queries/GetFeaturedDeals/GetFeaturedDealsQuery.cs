using MediatR;

namespace TABP.Application.Hotels.Queries.GetFeaturedDeals;
public class GetFeaturedDealsQuery : IRequest<IEnumerable<FeaturedDealResponse>>
{
    public int NumberOfDeals { get; set; }
}