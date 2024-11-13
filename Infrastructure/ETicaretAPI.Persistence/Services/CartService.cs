using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Application.Repositories.Cart;
using ETicaretAPI.Application.Repositories.CartItem;
using ETicaretAPI.Application.ViewModels.Cart;
using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ETicaretAPI.Persistence.Services;

public class CartService : ICartService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<AppUser> _userManager;
    private readonly IOrderReadRepository _orderReadRepository;
    private readonly ICartWriteRepository _cartWriteRepository;
    private readonly ICartReadRepository _cartReadRepository;
    private readonly ICartItemWriteRepository _cartItemWriteRepository;
    private readonly ICartItemReadRepository _cartItemReadRepository;

    public CartService(IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager,
        IOrderReadRepository orderReadRepository, ICartWriteRepository cartWriteRepository,
        ICartItemWriteRepository cartItemWriteRepository, ICartItemReadRepository cartItemReadRepository,
        ICartReadRepository cartReadRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
        _orderReadRepository = orderReadRepository;
        _cartWriteRepository = cartWriteRepository;
        _cartItemWriteRepository = cartItemWriteRepository;
        _cartItemReadRepository = cartItemReadRepository;
        _cartReadRepository = cartReadRepository;
    }

    private async Task<Cart?> ContextUser()
    {
        var username = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
        if (!string.IsNullOrEmpty(username))
        {
            var user = await _userManager.Users
                .Include(u => u.Carts)
                .FirstOrDefaultAsync(u => u.UserName == username);

            var basket = from cart in user.Carts
                join order in _orderReadRepository.Table
                    on cart.Id equals order.Id into CartOrders
                from order in CartOrders.DefaultIfEmpty()
                select new
                {
                    Cart = cart,
                    Order = order
                };

            Cart? targetCart = null;

            if (basket.Any(x => x.Order is null))
                targetCart = basket.FirstOrDefault(x => x.Order is null)?.Cart;
            else
            {
                targetCart = new Cart();
                user.Carts.Add(targetCart);
            }


            await _cartWriteRepository.SaveAsync();
            return targetCart;
        }

        throw new Exception("Unexpected error occurred while getting user information.");
    }

    public async Task<List<CartItem>> GetCartItemsAsync()
    {
        var cart = await ContextUser();
        var result = await _cartReadRepository.Table
            .Include(x => x.CartItems)
            .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(x => x.Id == cart.Id);

        return result.CartItems.ToList();
    }

    public async Task AddItemToCartAsync(VMCreateCartItem cartItem)
    {
        var cart = await ContextUser();

        if (cart != null)
        {
            var basketItem = await _cartItemReadRepository.GetSingleAsync(x =>
                x.CartId == cart.Id && x.ProductId == Guid.Parse(cartItem.ProductId));

            if (basketItem.Product != null)
            {
                basketItem.Quantity++;
            }
            else
            {
                await _cartItemWriteRepository.AddAsync(new()
                {
                    CartId = cart.Id,
                    ProductId = Guid.Parse(cartItem.ProductId),
                    Quantity = cartItem.Quantity
                });
            }

            await _cartItemWriteRepository.SaveAsync();
        }
    }

    public async Task UpdateCartItemAsync(VMUpdateCartItem cartItem)
    {
        var basketItem = await _cartItemReadRepository.GetByIdAsync(cartItem.CartItemId);
        if (basketItem != null)
        {
            basketItem.Quantity = cartItem.Quantity;
            await _cartItemWriteRepository.SaveAsync();
        }
    }

    public async Task RemoveCartItemAsync(string cartItemId)
    {
        var cartItem = await _cartItemReadRepository.GetByIdAsync(cartItemId);

        if (cartItem != null)
        {
            _cartItemWriteRepository.Remove(cartItem);
            await _cartItemWriteRepository.SaveAsync();
        }
    }

    public Cart? GetUserActiveCart
    {
        get
        {
            var cart = ContextUser().Result;
            return cart;
        }
    }
}