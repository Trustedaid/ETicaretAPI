using ETicaretAPI.Application.Repositories.File;
using ETicaretAPI.Persistence.Contexts;
using File = ETicaretAPI.Domain.Entities.File;

namespace ETicaretAPI.Persistence.Repositories;

public class FileReadRepository : ReadRepository<File>, IFileReadRepository
{
    public FileReadRepository(ETicaretAPIDbContext context) : base(context)
    {
    }
}