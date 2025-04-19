using Asp.Versioning;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TABP.Application.Owners.Commands.Create;
using TABP.Application.Owners.Commands.Delete;
using TABP.Application.Owners.Queries.GetById;
using TABP.Domain.Constants;
using TABP.Presentation.DTOs.Owner;

namespace TABP.Presentation.Controllers;

/// <summary>
/// Controller for managing owner-related operations, including creating, retrieving, and deleting owners.
/// </summary>
[ApiVersion(1)]
[Route("api/v{version:apiVersion}/owners")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
public class OwnersController(IMediator mediator, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Creates a new owner by providing necessary details.
    /// </summary>
    /// <param name="request">The request containing the owner details to be created.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The newly created owner.</returns>
    /// <response code="201">Successfully created the owner.</response>
    /// <response code="400">Invalid input data.</response>
    /// <response code="401">The user is not authenticated.</response>
    /// <response code="403">The user does not have permission to create an owner.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateOwner(
      CreateOwnerRequest request,
      CancellationToken cancellationToken)
    {
        var command = mapper.Map<CreateOwnerCommand>(request);

        var createdOwner = await mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetOwner), new { id = createdOwner.Id }, createdOwner);
    }

    /// <summary>
    /// Retrieves an owner by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the owner.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The owner details corresponding to the specified ID.</returns>
    /// <response code="200">Successfully retrieved the owner.</response>
    /// <response code="400">Invalid owner ID format.</response>
    /// <response code="401">The user is not authenticated.</response>
    /// <response code="403">The user does not have permission to access the owner data.</response>
    /// <response code="404">No owner found with the specified ID.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetOwner(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetOwnerByIdQuery(id);

        var owner = await mediator.Send(query, cancellationToken);

        return Ok(owner);
    }

    /// <summary>
    /// Deletes an existing owner by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the owner to be deleted.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>No content if deletion is successful.</returns>
    /// <response code="204">Successfully deleted the owner.</response>
    /// <response code="400">Invalid owner ID format.</response>
    /// <response code="401">The user is not authenticated.</response>
    /// <response code="403">The user does not have permission to delete the owner.</response>
    /// <response code="404">No owner found with the specified ID to delete.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteOwner(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteOwnerCommand(id);

        await mediator.Send(command, cancellationToken);

        return NoContent();
    }
}
