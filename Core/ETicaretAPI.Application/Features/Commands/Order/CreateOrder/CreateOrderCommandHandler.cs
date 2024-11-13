using ETicaretAPI.Application.Abstractions.Hubs;
using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.DTOs.Order;
using MediatR;

namespace ETicaretAPI.Application.Features.Commands.Order.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommandRequest, CreateOrderCommandResponse>
{
    private readonly IOrderService _orderService;
    private readonly ICartService _cartService;
    private readonly IOrderHubService _orderHubService;

    public CreateOrderCommandHandler(IOrderService orderService, ICartService cartService, IOrderHubService orderHubService)
    {
        _orderService = orderService;
        _cartService = cartService;
        _orderHubService = orderHubService;
    }

    public async Task<CreateOrderCommandResponse> Handle(CreateOrderCommandRequest request, CancellationToken cancellationToken)
    {
       await _orderService.CreateOrderAsync(new CreateOrderDto()
        {
            Address = request.Address,
            Description = request.Description,
            CartId = _cartService.GetUserActiveCart?.Id.ToString()
        });

       await _orderHubService.OrderAddedMessageAsync("A new order received!");
        return new CreateOrderCommandResponse();
    }
}