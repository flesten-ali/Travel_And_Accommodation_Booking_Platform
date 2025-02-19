using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;
using TABP.Application.CartItems.Commands.Create;
using TABP.Application.CartItems.Commands.Delete;
using TABP.Application.CartItems.Queries.GetAll;
using TABP.Domain.Constants;
using TABP.Presentation.DTOs.CartItem;
namespace TABP.Presentation.Controllers;

[Route("api/users/{userId:guid}/cart-items")]
[ApiController]
[Authorize(Roles = Roles.Guest)]
[SwaggerTag("Manage shopping cart items for guests.")]  
public class CartItemsController(IMediator mediator, IMapper mapper) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly IMapper _mapper = mapper;

    [HttpPost]
    [SwaggerOperation(
        Summary = "Add an item to the cart",
        Description = "Adds a new item to the guest's shopping cart."
    )]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateCartItem(
        Guid userId,
        Guid roomClassId,
        CancellationToken cancellationToken)
    {
        var command = new CreateCartItemCommand
        {
            UserId = userId,
            RoomClassId = roomClassId
        };

        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [SwaggerOperation(
     Summary = "Delete a cart item",
     Description = "Delete an existing cart item by its unique ID."
    )]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteCartItem(Guid userId, Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteCartItemCommand
        {
            CartId = id,
            UserId = userId,
        };

        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get cart items for specific guest",
        Description = "Retrieve a list of cart items for specific guest."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCartItems(
        Guid userId,
        [FromQuery] GetCartItemsRequest request,
        CancellationToken cancellationToken)
    {
        var query = _mapper.Map<GetCartItemsQuery>(request);
        query.UserId = userId;

        var cartItems = await _mediator.Send(query, cancellationToken);

        Response.Headers.Append("x-pagination",
            JsonSerializer.Serialize(cartItems.PaginationMetaData));

        return Ok(cartItems.Items);
    }
}