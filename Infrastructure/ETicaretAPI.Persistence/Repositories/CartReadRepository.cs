using ETicaretAPI.Application.Repositories.Cart;
using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Persistence.Contexts;

namespace ETicaretAPI.Persistence.Repositories;

public class CartReadRepository : ReadRepository<Cart>, ICartReadRepository
{
    public CartReadRepository(ETicaretAPIDbContext context) : base(context)
    {
    }
}