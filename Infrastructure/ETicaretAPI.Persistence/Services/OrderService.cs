using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.DTOs.Order;
using ETicaretAPI.Application.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ETicaretAPI.Persistence.Services;

public class OrderService : IOrderService
{
    private readonly IOrderWriteRepository _orderWriteRepository;
    private readonly IOrderReadRepository _orderReadRepository;

    public OrderService(IOrderWriteRepository orderWriteRepository, IOrderReadRepository orderReadRepository)
    {
        _orderWriteRepository = orderWriteRepository;
        _orderReadRepository = orderReadRepository;
    }

    public async Task CreateOrderAsync(CreateOrderDto createOrder)
    {
        var orderCode = (new Random().NextDouble() * 10000).ToString();
        orderCode = orderCode.Substring(orderCode.IndexOf(".") + 1, orderCode.Length - orderCode.IndexOf(".") - 1);

        await _orderWriteRepository.AddAsync(new()
        {
            Address = createOrder.Address,
            Id = Guid.Parse(createOrder.CartId),
            Description = createOrder.Description,
            OrderCode = orderCode
        });
        await _orderWriteRepository.SaveAsync();
    }

    public async Task<ListOrder> GetAllOrdersAsync(int page, int size)
    {
        var query = _orderReadRepository.Table.Include(o => o.Cart)
            .ThenInclude(c => c.User)
            .Include(o => o.Cart)
            .ThenInclude(c => c.CartItems)
            .ThenInclude(ci => ci.Product);

        var data = query.Skip(page * size).Take(size);
        // .Take((page *size)..size)


        return new()
        {
            TotalOrderCount = await query.CountAsync(),
            Orders = await data.Select(o => new
            {
                Id = o.Id,
                CreatedDate = o.CreatedDate,
                OrderCode = o.OrderCode,
                TotalPrice = o.Cart.CartItems.Sum(ci => ci.Product.Price * ci.Quantity),
                Username = o.Cart.User.UserName
            }).ToListAsync()
        };
    }

    public async Task<SingleOrder> GetOrderByIdAsync(string id)
    {
        var data = await _orderReadRepository.Table
            .Include(o => o.Cart)
            .ThenInclude(c => c.CartItems)
            .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(o => o.Id == Guid.Parse(id));


        return new()
        {
            Id = data.Id.ToString(),
            CartItems = data.Cart.CartItems.Select(ci => new
            {
                ci.Product.Name,
                ci.Product.Price,
                ci.Quantity
            }),
            Address = data.Address,
            CreatedDate = data.CreatedDate,
            Description = data.Description,
            OrderCode = data.OrderCode
        };
    }
}