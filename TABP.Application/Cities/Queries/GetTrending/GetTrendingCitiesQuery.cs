using MediatR;

namespace TABP.Application.Cities.Queries.GetTrending;
public sealed record GetTrendingCitiesQuery(int Limit) : IRequest<IEnumerable<TrendingCitiesResponse>>;
