using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TABP.Application.CartItems.AddToCart;
using TABP.Domain.Constants;
using TABP.Presentation.DTOs.CartItem;
namespace TABP.Presentation.Controllers;

[Route("api/cart-items")]
[ApiController]
[Authorize(Roles = Roles.Guest)]
public class CartItemsController(IMediator mediator, IMapper mapper) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly IMapper _mapper = mapper;

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
    {
        var command = _mapper.Map<AddToCartCommand>(request);

        await _mediator.Send(command);

        return NoContent();
    }
}