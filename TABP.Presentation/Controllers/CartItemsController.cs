﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TABP.Application.CartItems.AddToCart;
using TABP.Domain.Constants;
using TABP.Presentation.DTOs.CartItem;
namespace TABP.Presentation.Controllers;

[Route("api/cart-items")]
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
    public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<AddToCartCommand>(request);

        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }
}