using ETicaretAPI.Application.Abstractions.Services;
using MediatR;

namespace ETicaretAPI.Application.Features.Commands.Cart.AddItemToCart;

public class AddItemToCartCommandHandler : IRequestHandler<AddItemToCartCommandRequest, AddItemToCartCommandResponse>
{
    private readonly ICartService _cartService;

    public AddItemToCartCommandHandler(ICartService cartService)
    {
        _cartService = cartService;
    }

    public async Task<AddItemToCartCommandResponse> Handle(AddItemToCartCommandRequest request, CancellationToken cancellationToken)
    {
        
        await _cartService.AddItemToCartAsync(new()
        {
            ProductId = request.ProductId,
            Quantity = request.Quantity
        });
        
        return new AddItemToCartCommandResponse();

    }
}