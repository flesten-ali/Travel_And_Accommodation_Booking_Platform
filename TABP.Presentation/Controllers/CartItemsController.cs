﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TABP.Application.CartItems.Commands.Create;
using TABP.Application.CartItems.Commands.Delete;
using TABP.Application.CartItems.Queries.GetAll;
using TABP.Domain.Constants;
using TABP.Presentation.DTOs.CartItem;
using TABP.Presentation.Extensions;
namespace TABP.Presentation.Controllers;

[Route("api/user/cart-items")]
[ApiController]
[Authorize(Roles = Roles.Guest)]
[SwaggerTag("Manage shopping cart items for guests.")]
public class CartItemsController(IMediator mediator, IMapper mapper) : ControllerBase
{
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
        CreateCartItemRequest request,
        CancellationToken cancellationToken)
    {
        var command = mapper.Map<CreateCartItemCommand>(request);
        command.UserId = User.GetUserId();

        await mediator.Send(command, cancellationToken);

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
    public async Task<IActionResult> DeleteCartItem(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteCartItemCommand
        {
            CartId = id,
            UserId = User.GetUserId(),
        };

        await mediator.Send(command, cancellationToken);

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
        [FromQuery] GetCartItemsRequest request,
        CancellationToken cancellationToken)
    {
        var query = mapper.Map<GetCartItemsQuery>(request);
        query.UserId = User.GetUserId();

        var cartItems = await mediator.Send(query, cancellationToken);

        Response.AddPaginationHeader(cartItems.PaginationMetaData);

        return Ok(cartItems.Items);
    }
}