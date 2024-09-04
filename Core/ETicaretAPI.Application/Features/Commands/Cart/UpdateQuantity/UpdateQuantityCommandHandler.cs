using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.ViewModels.Cart;
using MediatR;

namespace ETicaretAPI.Application.Features.Commands.Cart.UpdateQuantity;

public class UpdateQuantityCommandHandler : IRequestHandler<UpdateQuantityCommandRequest, UpdateQuantityCommandResponse>
{
    private readonly ICartService _cartService;

    public UpdateQuantityCommandHandler(ICartService cartService)
    {
        _cartService = cartService;
    }

    public async Task<UpdateQuantityCommandResponse> Handle(UpdateQuantityCommandRequest request, CancellationToken cancellationToken)
    {
         await _cartService.UpdateCartItemAsync(new VMUpdateCartItem()
        {
            CartItemId = request.CartItemId,
            Quantity = request.Quantity
        });
        return new UpdateQuantityCommandResponse();
    }
}