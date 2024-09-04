using MediatR;

namespace ETicaretAPI.Application.Features.Commands.Cart.AddItemToCart;

public class AddItemToCartCommandRequest : IRequest<AddItemToCartCommandResponse>
{
    public string ProductId { get; set; }
    public int Quantity { get; set; }
}