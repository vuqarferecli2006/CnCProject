using CnC.Application.Abstracts.Repositories.ICurrencyRateRepository;
using CnC.Domain.Entities;
using CnC.Percistance.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CnC.Percistance.Repositories;

public class CurrencyRateRepository : Repository<CurrencyRate>, ICurrencyRateReadRepository, ICurrencyRateWriteRepository
{
    private readonly AppDbContext _context;
    public CurrencyRateRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<CurrencyRate?> GetCurrencyRateByCodeAsync(string currencyCode, CancellationToken cancellationToken)
    {
        return await _context.CurrencyRates
            .FirstOrDefaultAsync(cr => cr.CurrencyCode == currencyCode, cancellationToken);
    }
}
