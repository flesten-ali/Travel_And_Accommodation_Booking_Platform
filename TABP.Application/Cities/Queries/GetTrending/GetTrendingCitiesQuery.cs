using MediatR;

namespace TABP.Application.Cities.Queries.GetTrending;
public class GetTrendingCitiesQuery : IRequest<IEnumerable<TrendingCitiesResponse>>
{
    public int Limit { get; set; }
}