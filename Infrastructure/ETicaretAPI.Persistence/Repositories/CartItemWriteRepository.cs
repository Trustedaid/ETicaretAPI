using ETicaretAPI.Application.Repositories.CartItem;
using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Persistence.Contexts;

namespace ETicaretAPI.Persistence.Repositories;

public class CartItemWriteRepository : WriteRepository<CartItem>, ICartItemWriteRepository
{
    public CartItemWriteRepository(ETicaretAPIDbContext context) : base(context)
    {
    }
}