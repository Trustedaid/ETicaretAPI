using ETicaretAPI.Application.Abstractions.Services;
using MediatR;

namespace ETicaretAPI.Application.Features.Queries.Cart.GetCartItems;

public class GetCartItemsQueryHandler : IRequestHandler<GetCartItemsQueryRequest, List<GetCartItemsQueryResponse>>
{
    private readonly ICartService _cartService;

    public GetCartItemsQueryHandler(ICartService cartService)
    {
        _cartService = cartService;
    }

    public async Task<List<GetCartItemsQueryResponse>> Handle(GetCartItemsQueryRequest request,
        CancellationToken cancellationToken)
    {
        var cartItems = await _cartService.GetCartItemsAsync();

        var result = cartItems.Select(x => new GetCartItemsQueryResponse()
        {
            CartItemId = x.Id.ToString(),
            Name = x.Product.Name,
            Price = x.Product.Price,
            Quantity = x.Quantity
        }).ToList();
        
        
        return result;
    }
}