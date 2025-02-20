using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TABP.Application.Owners.Commands.Create;
using TABP.Application.Owners.Queries.GetById;
using TABP.Presentation.DTOs.Owner;

namespace TABP.Presentation.Controllers;
[Route("api/owners")]
[ApiController]
//[Authorize(Roles = Roles.Admin)] // deleete
public class OwnersController(IMediator mediator, IMapper mapper) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly IMapper _mapper = mapper;

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
      [FromBody] CreateOwnerRequset request,
      CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreateOwnerCommand>(request);

        var createdOwner = await _mediator.Send(command, cancellationToken);

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
        var query = new GetOwnerByIdQuery { OwnerId = id };

        var owner = await _mediator.Send(query, cancellationToken);

        return Ok(owner);
    }
}
