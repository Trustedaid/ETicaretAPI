using ETicaretAPI.Application.Repositories.Cart;
using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Persistence.Contexts;

namespace ETicaretAPI.Persistence.Repositories;

public class CartWriteRepository : WriteRepository<Cart>, ICartWriteRepository
{
    public CartWriteRepository(ETicaretAPIDbContext context) : base(context)
    {
    }
}