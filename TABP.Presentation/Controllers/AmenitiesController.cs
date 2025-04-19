using Asp.Versioning;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TABP.Application.Amenities.Commands.Create;
using TABP.Application.Amenities.Commands.Delete;
using TABP.Application.Amenities.Commands.Update;
using TABP.Application.Amenities.Common;
using TABP.Application.Amenities.Queries.GetById;
using TABP.Domain.Constants;
using TABP.Presentation.DTOs.Amenity;

namespace TABP.Presentation.Controllers;

/// <summary>
/// Controller for managing amenities in the system.
/// </summary>
[ApiVersion(1)]
[Route("api/v{version:apiVersion}/amenities")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
public class AmenitiesController(IMediator mediator, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Creates a new amenity.
    /// </summary>
    /// <param name="request">The details of the amenity to be created.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>The newly created amenity.</returns>
    /// <response code="201">The amenity was successfully created.</response>
    /// <response code="403">The user does not have permission to create an amenity.</response>
    /// <response code="400">The request data is invalid.</response>
    /// <response code="401">The user is not authenticated.</response>
    [HttpPost]
    [ProducesResponseType(typeof(AmenityResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateAmenity(CreateAmenityRequest request, CancellationToken cancellationToken)
    {
        var command = mapper.Map<CreateAmenityCommand>(request);

        var createdAmenity = await mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetAmenity), new { id = createdAmenity.Id }, createdAmenity);
    }

    /// <summary>
    /// Retrieves an amenity by its unique ID.
    /// </summary>
    /// <param name="id">The unique identifier of the amenity.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>The amenity details if found.</returns>
    /// <response code="200">The amenity was successfully retrieved.</response>
    /// <response code="403">The user does not have permission to access the amenity.</response>
    /// <response code="401">The user is not authenticated.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AmenityResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAmenity(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetAmenityByIdQuery(id);

        var amenity = await mediator.Send(query, cancellationToken);

        return Ok(amenity);
    }

    /// <summary>
    /// Updates an existing amenity.
    /// </summary>
    /// <param name="id">The unique identifier of the amenity.</param>
    /// <param name="request">The updated details of the amenity.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>No content if update is successful.</returns>
    /// <response code="204">The amenity was successfully updated.</response>
    /// <response code="400">The request data is invalid.</response>
    /// <response code="403">The user does not have permission to update the amenity.</response>
    /// <response code="401">The user is not authenticated.</response>
    /// <response code="404">The specified amenity was not found.</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAmenity(Guid id, UpdateAmenityRequest request, CancellationToken cancellationToken)
    {
        var command = mapper.Map<UpdateAmenityCommand>(request);
        command.Id = id;

        await mediator.Send(command, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Deletes an amenity by its unique ID.
    /// </summary>
    /// <param name="id">The unique identifier of the amenity to be deleted.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>No content if deletion is successful.</returns>
    /// <response code="204">The amenity was successfully deleted.</response>
    /// <response code="400">The request data is invalid.</response>
    /// <response code="401">The user is not authenticated.</response>
    /// <response code="403">The user does not have permission to delete the amenity.</response>
    /// <response code="404">The specified amenity was not found.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteAmenity(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteAmenityCommand(id);

        await mediator.Send(command, cancellationToken);

        return NoContent();
    }
}