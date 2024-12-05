using ETicaretAPI.Application.Repositories.CompletedOrder;
using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Persistence.Contexts;

namespace ETicaretAPI.Persistence.Repositories;

public class CompletedOrderWriteRepository : WriteRepository<CompletedOrders>, ICompletedOrderWriteRepository
{
    public CompletedOrderWriteRepository(ETicaretAPIDbContext context) : base(context)
    {
    }
}