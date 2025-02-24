using Asp.Versioning;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TABP.Application.Cities.Commands.Create;
using TABP.Application.Cities.Commands.Delete;
using TABP.Application.Cities.Commands.Thumbnail;
using TABP.Application.Cities.Commands.Update;
using TABP.Application.Cities.Queries.GetById;
using TABP.Application.Cities.Queries.GetForAdmin;
using TABP.Application.Cities.Queries.GetTrending;
using TABP.Domain.Constants;
using TABP.Presentation.DTOs.City;
using TABP.Presentation.Extensions;

namespace TABP.Presentation.Controllers;

/// <summary>
/// Controller for managing city-related operations such as retrieval, creation, updating, and deletion.
/// </summary>
[ApiVersion(1)]
[Route("api/v{version:apiVersion}/cities")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
public class CitiesController(IMediator mediator, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Retrieves a list of trending cities based on popularity.
    /// </summary>
    /// <remarks>
    /// A city is considered trending if it has the highest number of booked hotels.
    /// </remarks>
    /// <param name="limit">The maximum number of cities to retrieve.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A list of trending cities based on hotel bookings.</returns>
    /// <response code="200">The list of trending cities was retrieved successfully.</response>
    /// <response code="400">The request data is invalid.</response>
    [HttpGet("trending-cities")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetTrendingCities(int limit, CancellationToken cancellationToken)
    {
        var query = new GetTrendingCitiesQuery(limit);

        var cities = await mediator.Send(query, cancellationToken);

        return Ok(cities);
    }

    /// <summary>
    /// Retrieves a paginated list of cities for admin users.
    /// </summary>
    /// <param name="request">The request containing pagination and filtering options.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A paginated list of cities.</returns>
    /// <response code="200">The list of cities was retrieved successfully.</response>
    /// <response code="401">The user is not authenticated.</response>
    /// <response code="403">The user does not have permission to access this data.</response>
    /// <response code="400">The request data is invalid.</response>
    [HttpGet("cities-for-admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCitiesForAdmin(
        [FromQuery] GetCitiesForAdminRequest request,
        CancellationToken cancellationToken)
    {
        var query = mapper.Map<GetCitiesForAdminQuery>(request);

        var cities = await mediator.Send(query, cancellationToken);

        Response.AddPaginationHeader(cities.PaginationMetaData);

        return Ok(cities.Items);
    }

    /// <summary>
    /// Adds a new city to the system.
    /// </summary>
    /// <param name="request">The city details to create.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>The created city details.</returns>
    /// <response code="201">The city was created successfully.</response>
    /// <response code="400">The request data is invalid.</response>
    /// <response code="401">The user is not authenticated.</response>
    /// <response code="403">The user does not have permission to create a city.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCity(CreateCityRequest request, CancellationToken cancellationToken)
    {
        var command = mapper.Map<CreateCityCommand>(request);

        var createdCity = await mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetCity), new { id = createdCity.Id }, createdCity);
    }

    /// <summary>
    /// Retrieves details of a specific city by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the city.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>The details of the specified city.</returns>
    /// <response code="200">The city details were retrieved successfully.</response>
    /// <response code="404">The specified city was not found.</response>
    /// <response code="400">The request data is invalid.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCity(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetCityByIdQuery(id);

        var city = await mediator.Send(query, cancellationToken);

        return Ok(city);
    }

    /// <summary>
    /// Removes a city from the system.
    /// </summary>
    /// <param name="id">The unique identifier of the city to delete.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>No content if deletion is successful.</returns>
    /// <response code="204">The city was successfully deleted.</response>
    /// <response code="400">The request data is invalid.</response>
    /// <response code="404">The specified city was not found.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteCity(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteCityCommand(id);

        await mediator.Send(command, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Updates an existing city's details.
    /// </summary>
    /// <param name="id">The unique identifier of the city to update.</param>
    /// <param name="request">The updated city details.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>No content if the update is successful.</returns>
    /// <response code="204">The city was successfully updated.</response>
    /// <response code="400">The request data is invalid.</response>
    /// <response code="404">The specified city was not found.</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateCity(Guid id, UpdateCityRequest request, CancellationToken cancellationToken)
    {
        var command = mapper.Map<UpdateCityCommand>(request);
        command.Id = id;

        await mediator.Send(command, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Uploads a thumbnail image for a specific city.
    /// </summary>
    /// <param name="id">The unique identifier of the city.</param>
    /// <param name="request">The thumbnail upload request containing the image.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>No content if the upload is successful.</returns>
    /// <response code="204">The thumbnail was successfully uploaded.</response>
    /// <response code="400">The request data is invalid.</response>
    /// <response code="404">The specified city was not found.</response>
    [HttpPost("{id:guid}/thumbnail")]
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
        var command = mapper.Map<UploadCityThumbnailCommand>(request);
        command.CityId = id;

        await mediator.Send(command, cancellationToken);

        return NoContent();
    }
}