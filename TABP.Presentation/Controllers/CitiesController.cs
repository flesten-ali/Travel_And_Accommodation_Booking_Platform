using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TABP.Application.Cities.Commands.Create;
using TABP.Application.Cities.Commands.Delete;
using TABP.Application.Cities.Commands.Thumbnail;
using TABP.Application.Cities.Commands.Update;
using TABP.Application.Cities.Queries.GetById;
using TABP.Application.Cities.Queries.GetForAdmin;
using TABP.Application.Cities.Queries.GetTrending;
using TABP.Domain.Constants;
using TABP.Presentation.DTOs.City;
namespace TABP.Presentation.Controllers;

[Route("api/cities")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
public class CitiesController(IMediator mediator, IMapper mapper) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly IMapper _mapper = mapper;

    [HttpGet("trending-cities")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetTrendingCities(int limit)
    {
        var query = new GetTrendingCitiesQuery { Limit = limit };

        var cities = await _mediator.Send(query);

        return Ok(cities);
    }

    [HttpGet("cities-for-admin")]
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

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCity([FromBody] CreateCityRequest request)
    {
        var command = _mapper.Map<CreateCityCommand>(request);

        var city = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetCity), new { id = city.Id }, city);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCity(Guid id)
    {
        var query = new GetCityByIdQuery { Id = id };

        var city = await _mediator.Send(query);

        return Ok(city);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteCity(Guid id)
    {
        var command = new DeleteCityCommand { Id = id };

        await _mediator.Send(command);

        return NoContent();
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateCity(Guid id, UpdateCityRequest request)
    {
        var command = _mapper.Map<UpdateCityCommand>(request);
        command.Id = id;

        await _mediator.Send(command);

        return NoContent();
    }

    [HttpPost("{id:guid}/thumbnail")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UploadCityThumbnail(Guid id, [FromForm] UploadCityThumbnailRequest request)
    {
        var command = _mapper.Map<UploadCityThumbnailCommand>(request);
        command.CityId = id;

        await _mediator.Send(command);

        return NoContent();
    }
}