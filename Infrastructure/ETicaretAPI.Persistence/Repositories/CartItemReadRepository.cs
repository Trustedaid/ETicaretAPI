using ETicaretAPI.Application.Repositories.CartItem;
using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Persistence.Contexts;

namespace ETicaretAPI.Persistence.Repositories;

public class CartItemReadRepository :  ReadRepository<CartItem>, ICartItemReadRepository
{
    public CartItemReadRepository(ETicaretAPIDbContext context) : base(context)
    {
    }
}