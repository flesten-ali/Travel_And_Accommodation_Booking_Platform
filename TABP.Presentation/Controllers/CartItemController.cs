using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TABP.Application.CartItems.AddToCart;
using TABP.Presentation.DTOs.CartItem;

namespace TABP.Presentation.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CartItemController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CartItemController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
    {
        var command = _mapper.Map<AddToCartCommand>(request);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}