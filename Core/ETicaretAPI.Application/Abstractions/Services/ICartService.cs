using ETicaretAPI.Application.ViewModels.Cart;
using ETicaretAPI.Domain.Entities;

namespace ETicaretAPI.Application.Abstractions.Services;

public interface ICartService
{
    public Task<List<CartItem>> GetCartItemsAsync();
    public Task AddItemToCartAsync(VMCreateCartItem cartItem);
    public Task UpdateCartItemAsync(VMUpdateCartItem cartItem);
    public Task RemoveCartItemAsync(string cartItemId);
    
}