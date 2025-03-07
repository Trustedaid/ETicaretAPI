﻿using ETicaretAPI.Application.Abstractions.Services;
using MediatR;

namespace ETicaretAPI.Application.Features.Queries.Order.GetOrderById;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQueryRequest, GetOrderByIdQueryResponse>
{
    private readonly IOrderService _orderService;

    public GetOrderByIdQueryHandler(IOrderService orderService)
    {
        _orderService = orderService;
    }


    public async Task<GetOrderByIdQueryResponse> Handle(GetOrderByIdQueryRequest request,
        CancellationToken cancellationToken)
    {
        var data = await _orderService.GetOrderByIdAsync(request.Id);
        return new GetOrderByIdQueryResponse()
        {
            Id = data.Id,
            Address = data.Address,
            CartItems = data.CartItems,
            CreatedDate = data.CreatedDate,
            Description = data.Description,
            OrderCode = data.OrderCode,
            Completed = data.Completed
        };
    }
}