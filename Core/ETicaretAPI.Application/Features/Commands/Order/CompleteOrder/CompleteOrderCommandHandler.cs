using ETicaretAPI.Application.Abstractions.Services;
using MediatR;

namespace ETicaretAPI.Application.Features.Commands.Order.CompleteOrder;

public class CompleteOrderCommandHandler : IRequestHandler<CompleteOrderCommandRequest, CompleteOrderCommandResponse>
{
    private readonly IOrderService _orderService;
    private readonly IMailService _mailService;

    public CompleteOrderCommandHandler(IOrderService orderService, IMailService mailService)
    {
        _orderService = orderService;
        _mailService = mailService;
    }

    public async Task<CompleteOrderCommandResponse> Handle(CompleteOrderCommandRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _orderService.CompleteOrderAsync(request.Id);
        if (result.Item1)
        {
            _mailService.SendCompletedOrderMailAsync(result.Item2.Email, result.Item2.OrderCode, result.Item2.OrderDate,
                result.Item2.Username, result.Item2.UserLastName);
        }

        return new CompleteOrderCommandResponse();
    }
}