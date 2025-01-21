using ETicaretAPI.Application.Consts;
using ETicaretAPI.Application.CustomAttributes;
using ETicaretAPI.Application.Enums;
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
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Carts, ActionType = ActionType.Reading,
            Definition = "Get Cart Items")]
        public async Task<IActionResult> GetCartItems([FromQuery] GetCartItemsQueryRequest getCartItemsQueryRequest)
        {
            var response = await _mediator.Send(getCartItemsQueryRequest);
            return Ok(response);
        }

        [HttpPost]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Carts, ActionType = ActionType.Writing,
            Definition = "Add Item To Cart")]
        public async Task<IActionResult> AddItemToCart(AddItemToCartCommandRequest addItemToCartCommandRequest)
        {
            var response = await _mediator.Send(addItemToCartCommandRequest);
            return Ok(response);
        }

        [HttpPut]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Carts, ActionType = ActionType.Updating,
            Definition = "Update Quantity")]
        public async Task<IActionResult> UpdateQuantity(UpdateQuantityCommandRequest updateQuantityCommandRequest)
        {
            var response = await _mediator.Send(updateQuantityCommandRequest);
            return Ok(response);
        }

        [HttpDelete("{CartItemId}")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Carts, ActionType = ActionType.Deleting,
            Definition = "Remove Cart Item")]
        public async Task<IActionResult> RemoveCartItem(
            [FromRoute] RemoveCartItemCommandRequest removeCartItemCommandRequest)
        {
            var response = await _mediator.Send(removeCartItemCommandRequest);
            return Ok(response);
        }
    }
}