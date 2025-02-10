using MediatR;

namespace TABP.Application.Cities.GetTrending;
public class GetTrendingCitiesQuery : IRequest<IEnumerable<TrendingCitiesResponse>>
{
    public int Limit { get; set; }
}