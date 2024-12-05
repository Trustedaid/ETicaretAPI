using ETicaretAPI.Application.Repositories.CompletedOrder;
using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Persistence.Contexts;

namespace ETicaretAPI.Persistence.Repositories;

public class CompletedOrderReadRepository : ReadRepository<CompletedOrders>,ICompletedOrderReadRepository
{
    public CompletedOrderReadRepository(ETicaretAPIDbContext context) : base(context)
    {
    }
}