using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TABP.Application.Owners.Commands.Create;
using TABP.Application.Owners.Commands.Delete;
using TABP.Application.Owners.Queries.GetById;
using TABP.Domain.Constants;
using TABP.Presentation.DTOs.Owner;

namespace TABP.Presentation.Controllers;
[Route("api/owners")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
public class OwnersController(IMediator mediator, IMapper mapper) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a new owner",
        Description = "Create a new owner by providing necessary details."
    )]
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

    [HttpGet("{id:guid}")]
    [SwaggerOperation(
        Summary = "Get owner by ID",
        Description = "Retrieve information of a owner by its unique identifier."
    )]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetOwner(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetOwnerByIdQuery(id) ;

        var owner = await mediator.Send(query, cancellationToken);

        return Ok(owner);
    }

    [HttpDelete("{id:guid}")]
    [SwaggerOperation(
    Summary = "Delete an owner",
    Description = "Delete an existing owner by its unique ID."
    )]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteOwner(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteOwnerCommand(id) ;

        await mediator.Send(command, cancellationToken);

        return NoContent();
    }
}
