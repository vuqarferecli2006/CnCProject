using CnC.Application.Abstracts.Services;
using CnC.Domain.Entities;
using CnC.Percistance.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CnC.Percistance;

public class CurrencyUpdateJob
{
    private readonly AppDbContext _context;
    private readonly ICurrencyService _currencyService;

    public CurrencyUpdateJob(AppDbContext context, ICurrencyService currencyService)
    {
        _context = context;
        _currencyService = currencyService;
    }

    public async Task UpdateRatesAsync()
    {
        var currencies = new[] { "USD", "EUR", "TRY" };

        var oldRates = await _context.CurrencyRates.ToListAsync();
        if (oldRates.Any())
        {
            _context.CurrencyRates.RemoveRange(oldRates);
            await _context.SaveChangesAsync();
        }

        foreach (var code in currencies)
        {
            var rate = await _currencyService.ConvertAsync(1, code, "AZN");

            var entity = new CurrencyRate
            {
                CurrencyCode = code,
                RateAgainstAzn = rate,
                Date = DateTime.UtcNow.Date,
                CreatedAt = DateTime.UtcNow
            };

            _context.CurrencyRates.Add(entity);
        }

        await _context.SaveChangesAsync();

        await UpdateProductCurrenciesAsync();
    }

    private async Task UpdateProductCurrenciesAsync()
    {
        var oldProductCurrencies = await _context.ProductCurrencies.ToListAsync();
        if (oldProductCurrencies.Any())
        {
            _context.ProductCurrencies.RemoveRange(oldProductCurrencies);
            await _context.SaveChangesAsync();
        }

        var todayRates = await _context.CurrencyRates
            .Where(c => c.Date == DateTime.UtcNow.Date)
            .ToListAsync();

        var products = await _context.Products
            .Where(p => !p.IsDeleted)
            .ToListAsync();

        foreach (var product in products)
        {
            decimal effectivePrice = product.PriceAzn;

            foreach (var rate in todayRates)
            {
                var convertedPrice = Math.Round(effectivePrice / rate.RateAgainstAzn, 1);

                _context.ProductCurrencies.Add(new ProductCurrency
                {
                    ProductId = product.Id,
                    CurrencyRateId = rate.Id,
                    ConvertedPrice = convertedPrice,
                    CreatedAt = DateTime.UtcNow
                });
            }
        }

        await _context.SaveChangesAsync();
    }
}
