using ETicaretAPI.Application.Repositories.File;
using ETicaretAPI.Persistence.Contexts;
using File = ETicaretAPI.Domain.Entities.File;

namespace ETicaretAPI.Persistence.Repositories;

public class FileWriteRepository : WriteRepository<File> , IFileWriteRepository
{
    public FileWriteRepository(ETicaretAPIDbContext context) : base(context)
    {
    }
}