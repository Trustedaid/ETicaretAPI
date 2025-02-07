using ETicaretAPI.Application.Repositories.Menu;
using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Persistence.Contexts;

namespace ETicaretAPI.Persistence.Repositories;

public class MenuWriteRepository : WriteRepository<Menu>, IMenuWriteRepository
{
    public MenuWriteRepository(ETicaretAPIDbContext context) : base(context)
    {
    }
}