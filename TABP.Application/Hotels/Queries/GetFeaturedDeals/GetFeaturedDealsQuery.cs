using MediatR;
namespace TABP.Application.Hotels.Queries.GetFeaturedDeals;

public class GetFeaturedDealsQuery : IRequest<IEnumerable<FeaturedDealResponse>>
{
    public int Limit { get; set; }
}