using CnC.Application.Abstracts.Repositories.IProductRepositories;
using CnC.Domain.Entities;
using CnC.Percistance.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CnC.Percistance.Repositories;

public class ProductRepository : Repository<Product>, IProductReadRepository, IProductWriteRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<CurrencyRate?> GetCurrencyRateByCodeAsync(string currencyCode)
    {
        return await _context.CurrencyRates
           .FirstOrDefaultAsync(cr => cr.CurrencyCode == currencyCode);
    }
}
