using ETicaretAPI.Application.Features.Commands.Cart.AddItemToCart;
using ETicaretAPI.Application.Features.Commands.Cart.RemoveCartItem;
using ETicaretAPI.Application.Features.Commands.Cart.UpdateQuantity;
using ETicaretAPI.Application.Features.Queries.Cart.GetCartItems;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class CartsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CartsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetCartItems([FromQuery]GetCartItemsQueryRequest getCartItemsQueryRequest)
        {
            var response = await _mediator.Send(getCartItemsQueryRequest);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddItemToCart(AddItemToCartCommandRequest addItemToCartCommandRequest)
        {
            var response = await _mediator.Send(addItemToCartCommandRequest);
            return Ok(response);
        }
        
        [HttpPut]
        public async Task<IActionResult> UpdateQuantity(UpdateQuantityCommandRequest updateQuantityCommandRequest)
        {
            var response = await _mediator.Send(updateQuantityCommandRequest);
            return Ok(response);
        }
        
        [HttpDelete("{CartItemId}")]
        public async Task<IActionResult> RemoveCartItem([FromRoute]RemoveCartItemCommandRequest removeCartItemCommandRequest)
        {
            var response = await _mediator.Send(removeCartItemCommandRequest);
            return Ok(response);
        }
        
    }
}
