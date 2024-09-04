using ETicaretAPI.Application.Abstractions.Services;
using MediatR;

namespace ETicaretAPI.Application.Features.Commands.Cart.RemoveCartItem;

public class RemoveCartItemCommandHandler : IRequestHandler<RemoveCartItemCommandRequest, RemoveCartItemCommandResponse>
{
    private readonly ICartService _cartService;

    public RemoveCartItemCommandHandler(ICartService cartService)
    {
        _cartService = cartService;
    }

    public async Task<RemoveCartItemCommandResponse> Handle(RemoveCartItemCommandRequest request,
        CancellationToken cancellationToken)
    {
        await _cartService.RemoveCartItemAsync(request.CartItemId);
        return new RemoveCartItemCommandResponse();
    }
}