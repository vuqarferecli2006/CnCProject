using CnC.Application.Abstracts.Repositories.IProductFilesRepository;
using CnC.Domain.Entities;
using CnC.Percistance.Contexts;

namespace CnC.Percistance.Repositories;

public class ProductFilesRepository : Repository<ProductFiles>, IProductFilesWriteRepository
{
    public ProductFilesRepository(AppDbContext context) : base(context)
    {
    }
}
