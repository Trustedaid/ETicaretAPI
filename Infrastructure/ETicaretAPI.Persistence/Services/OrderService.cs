using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.DTOs.Order;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Application.Repositories.CompletedOrder;
using ETicaretAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ETicaretAPI.Persistence.Services;

public class OrderService : IOrderService
{
    private readonly IOrderWriteRepository _orderWriteRepository;
    private readonly IOrderReadRepository _orderReadRepository;
    private readonly ICompletedOrderWriteRepository _completedOrderWriteRepository;
    private readonly ICompletedOrderReadRepository _completedOrderReadRepository;

    public OrderService(IOrderWriteRepository orderWriteRepository, IOrderReadRepository orderReadRepository,
        ICompletedOrderWriteRepository completedOrderWriteRepository,
        ICompletedOrderReadRepository completedOrderReadRepository)
    {
        _orderWriteRepository = orderWriteRepository;
        _orderReadRepository = orderReadRepository;
        _completedOrderWriteRepository = completedOrderWriteRepository;
        _completedOrderReadRepository = completedOrderReadRepository;
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


        var data2 = from order in data
            join completedOrder in _completedOrderReadRepository.Table on order.Id
                equals completedOrder.OrderId into co
            from _co in co.DefaultIfEmpty()
            select new
            {
                Id = order.Id,
                CreatedDate = order.CreatedDate,
                OrderCode = order.OrderCode,
                Cart = order.Cart,
                Completed = _co != null ? true : false
            };


        return new()
        {
            TotalOrderCount = await query.CountAsync(),
            Orders = await data2.Select(o => new
            {
                Id = o.Id,
                CreatedDate = o.CreatedDate,
                OrderCode = o.OrderCode,
                TotalPrice = o.Cart.CartItems.Sum(ci => ci.Product.Price * ci.Quantity),
                Username = o.Cart.User.UserName,
                o.Completed
            }).ToListAsync()
        };
    }

    public async Task<SingleOrder> GetOrderByIdAsync(string id)
    {
        var data = _orderReadRepository.Table
            .Include(o => o.Cart)
            .ThenInclude(c => c.CartItems)
            .ThenInclude(ci => ci.Product);

        var data2 = await (from order in data
                join completedOrder in _completedOrderReadRepository.Table
                    on order.Id equals completedOrder.OrderId into co
                from _co in co.DefaultIfEmpty()
                select new
                {
                    Id = order.Id,
                    CreatedDate = order.CreatedDate,
                    OrderCode = order.OrderCode,
                    Cart = order.Cart,
                    Completed = _co != null ? true : false,
                    Address = order.Address,
                    Description = order.Description
                })
            .FirstOrDefaultAsync(o => o.Id == Guid.Parse(id));


        return new()
        {
            Id = data2.Id.ToString(),
            CartItems = data2.Cart.CartItems.Select(ci => new
            {
                ci.Product.Name,
                ci.Product.Price,
                ci.Quantity
            }),
            Address = data2.Address,
            CreatedDate = data2.CreatedDate,
            Description = data2.Description,
            OrderCode = data2.OrderCode,
            Completed = data2.Completed
        };
    }

    public async Task<(bool, CompletedOrderDto)> CompleteOrderAsync(string id)
    {
        var order = await _orderReadRepository.Table.Include(o => o.Cart)
            .ThenInclude(c => c.User)
            .FirstOrDefaultAsync(o => o.Id == Guid.Parse(id));

        if (order != null)
        {
            await _completedOrderWriteRepository.AddAsync(new CompletedOrders()
                { OrderId = Guid.Parse(id) }
            );
            return (await _completedOrderWriteRepository.SaveAsync() > 0, new CompletedOrderDto()
            {
                OrderCode = order.OrderCode,
                OrderDate = order.CreatedDate,
                Username = order.Cart.User.UserName,
                UserLastName = order.Cart.User.LastName,
                Email = order.Cart.User.Email
            });
        }

        return (false, null);
    }
}