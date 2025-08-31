using CnC.Application.Abstracts.Repositories.IProductCurrencyRepository;
using CnC.Domain.Entities;
using CnC.Percistance.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CnC.Percistance.Repositories;

public class ProductCurrencyWriteRepository : Repository<ProductCurrency>, IProductCurrencyWriteRepository
{
    private readonly AppDbContext _context;
   
    public ProductCurrencyWriteRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductCurrency>> GetByProductIdAsync(Guid productId)
    {
        return await _context.ProductCurrencies
            .Where(c => c.ProductId == productId && !c.IsDeleted) 
            .ToListAsync();
    }
}