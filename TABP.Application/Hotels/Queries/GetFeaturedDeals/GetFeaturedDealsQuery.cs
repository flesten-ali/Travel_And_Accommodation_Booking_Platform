using MediatR;
namespace TABP.Application.Hotels.Queries.GetFeaturedDeals;

public sealed record GetFeaturedDealsQuery(int Limit) : IRequest<IEnumerable<FeaturedDealResponse>>;
