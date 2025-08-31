using CnC.Application.Abstracts.Repositories.IProductRepositories;
using CnC.Domain.Entities;
using CnC.Percistance.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace CnC.Percistance.Repositories;

public class ProductRepository : Repository<Product>, IProductReadRepository, IProductWriteRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Product?> GetByIdWithCurrenciesAsync(Guid id, CancellationToken ct=default)
    {
        return await _context.Products
            .Include(p => p.ProductCurrencies) 
            .Include(p=>p.ProductDescription)
            .FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public async Task<CurrencyRate?> GetCurrencyRateByCodeAsync(string currencyCode)
    {
        return await _context.CurrencyRates
           .FirstOrDefaultAsync(cr => cr.CurrencyCode == currencyCode);
    }

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Products
            .Include(p => p.ProductDescription)  
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }
}
