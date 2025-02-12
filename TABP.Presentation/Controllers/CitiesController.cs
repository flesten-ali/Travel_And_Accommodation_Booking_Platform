using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TABP.Application.Cities.GetTrending;
using TABP.Application.Cities.Queries.GetForAdmin;
using TABP.Domain.Constants;
using TABP.Presentation.DTOs.City;
namespace TABP.Presentation.Controllers;

[Route("api/cities")]
[ApiController]
public class CitiesController(IMediator mediator, IMapper mapper) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly IMapper _mapper = mapper;

    [HttpGet("trending-cities")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetTrendingCities(int limit)
    {
        var query = new GetTrendingCitiesQuery { Limit = limit };

        var cities = await _mediator.Send(query);

        return Ok(cities);
    }

    [HttpGet("cities-for-admin")]
    [Authorize(Roles = Roles.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCitiesForAdmin([FromQuery] GetCitiesForAdminRequest request)
    {
        var query = _mapper.Map<GetCitiesForAdminQuery>(request);

        var cities = await _mediator.Send(query);

        Response.Headers.Append("x-pagination",
            JsonSerializer.Serialize(cities.PaginationMetaData));

        return Ok(cities.Items);
    }
}
