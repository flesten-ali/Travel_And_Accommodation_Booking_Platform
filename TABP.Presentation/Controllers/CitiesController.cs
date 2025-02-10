using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TABP.Application.Cities.GetTrending;
namespace TABP.Presentation.Controllers;

[Route("api/cities")]
[ApiController]
public class CitiesController(IMediator mediator, IMapper mapper) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTrendingCities(int limit)
    {
        var query = new GetTrendingCitiesQuery { Limit = limit };

        var cities = await _mediator.Send(query);

        return Ok(cities);
    }
}
