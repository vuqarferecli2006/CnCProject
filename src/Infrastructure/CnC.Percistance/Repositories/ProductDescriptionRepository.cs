using CnC.Application.Abstracts.Repositories.IProductDescriptionRepository;
using CnC.Domain.Entities;
using CnC.Percistance.Contexts;

namespace CnC.Percistance.Repositories;

public class ProductDescriptionRepository : Repository<ProductDescription>, IProductDescriptionWriteRepository, IProductDescriptionReadRepository
{
    public ProductDescriptionRepository(AppDbContext context) : base(context)
    {
    }
}
