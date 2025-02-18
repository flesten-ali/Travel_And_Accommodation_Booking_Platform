using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
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
[SwaggerTag("Manage city-related operations including retrieval, creation, update, and deletion.")]
public class CitiesController(IMediator mediator, IMapper mapper) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly IMapper _mapper = mapper;

    [HttpGet("trending-cities")]
    [SwaggerOperation(
        Summary = "Get trending cities",
        Description = "Retrieves a list of trending cities based on popularity."
    )]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetTrendingCities(int limit, CancellationToken cancellationToken)
    {
        var query = new GetTrendingCitiesQuery { Limit = limit };

        var cities = await _mediator.Send(query, cancellationToken);

        return Ok(cities);
    }

    [HttpGet("cities-for-admin")]
    [SwaggerOperation(
        Summary = "Get cities for admin",
        Description = "Retrieves a paginated list of cities for admin users."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCitiesForAdmin(
        [FromQuery] GetCitiesForAdminRequest request,
        CancellationToken cancellationToken)
    {
        var query = _mapper.Map<GetCitiesForAdminQuery>(request);

        var cities = await _mediator.Send(query, cancellationToken);

        Response.Headers.Append("x-pagination",
            JsonSerializer.Serialize(cities.PaginationMetaData));

        return Ok(cities.Items);
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a city",
        Description = "Adds a new city to the system."
    )]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCity([FromBody] CreateCityRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreateCityCommand>(request);

        var createdCity = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetCity), new { id = createdCity.Id }, createdCity);
    }

    [HttpGet("{id:guid}")]
    [SwaggerOperation(
        Summary = "Get city by ID",
        Description = "Retrieves details of a specific city by its unique identifier."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCity(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetCityByIdQuery { Id = id };

        var city = await _mediator.Send(query, cancellationToken);

        return Ok(city);
    }

    [HttpDelete("{id:guid}")]
    [SwaggerOperation(
        Summary = "Delete city",
        Description = "Removes a city from the system."
    )]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteCity(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteCityCommand { Id = id };

        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpPut("{id:guid}")]
    [SwaggerOperation(
        Summary = "Update city",
        Description = "Updates an existing city's details."
    )]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateCity(Guid id, UpdateCityRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<UpdateCityCommand>(request);
        command.Id = id;

        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpPost("{id:guid}/thumbnail")]
    [SwaggerOperation(
        Summary = "Upload city thumbnail",
        Description = "Uploads a thumbnail image for a specific city."
    )]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UploadCityThumbnail(
        Guid id,
        [FromForm] UploadCityThumbnailRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<UploadCityThumbnailCommand>(request);
        command.CityId = id;

        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }
}