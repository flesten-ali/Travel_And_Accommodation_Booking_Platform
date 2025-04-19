using Asp.Versioning;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TABP.Application.CartItems.Commands.Create;
using TABP.Application.CartItems.Commands.Delete;
using TABP.Application.CartItems.Queries.GetAll;
using TABP.Domain.Constants;
using TABP.Presentation.DTOs.CartItem;
using TABP.Presentation.Extensions;

namespace TABP.Presentation.Controllers;

/// <summary>
/// Controller for managing shopping cart items for guests.
/// </summary>
[ApiVersion(1)]
[Route("api/v{version:apiVersion}/user/cart-items")]
[ApiController]
[Authorize(Roles = Roles.Guest)]
public class CartItemsController(IMediator mediator, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Adds a new item to the guest's shopping cart.
    /// </summary>
    /// <param name="request">The details of the cart item to be added.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>No content response if the item is added successfully.</returns>
    /// <response code="200">The cart item was successfully added.</response>
    /// <response code="400">The request data is invalid.</response>
    /// <response code="401">The user is not authenticated.</response>
    /// <response code="403">The user does not have permission to add a cart item.</response>
    [HttpPost]
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

    /// <summary>
    /// Deletes an existing cart item by its unique ID.
    /// </summary>
    /// <param name="id">The unique identifier of the cart item to be deleted.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>No content response if the item is successfully deleted.</returns>
    /// <response code="204">The cart item was successfully deleted.</response>
    /// <response code="400">The request data is invalid.</response>
    /// <response code="401">The user is not authenticated.</response>
    /// <response code="403">The user does not have permission to delete a cart item.</response>
    /// <response code="404">The specified cart item was not found.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteCartItem(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteCartItemCommand(id, User.GetUserId());

        await mediator.Send(command, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Retrieves a list of cart items for a specific guest.
    /// </summary>
    /// <param name="request">The request containing filter parameters for retrieving cart items.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A list of cart items along with pagination metadata.</returns>
    /// <response code="200">The cart items were successfully retrieved.</response>
    /// <response code="400">The request data is invalid.</response>
    /// <response code="401">The user is not authenticated.</response>
    /// <response code="403">The user does not have permission to view cart items.</response>
    /// <response code="404">No cart items found for the specified user.</response>
    [HttpGet]
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