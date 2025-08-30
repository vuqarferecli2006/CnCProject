using CnC.Application.Abstracts.Repositories.IProductCurrencyRepository;
using CnC.Domain.Entities;
using CnC.Percistance.Contexts;

namespace CnC.Percistance.Repositories;

public class ProductCurrencyWriteRepository : Repository<ProductCurrency>, IProductCurrencyWriteRepository
{
    public ProductCurrencyWriteRepository(AppDbContext context) : base(context)
    {
    }
}