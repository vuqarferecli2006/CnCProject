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

        foreach (var code in currencies)
        {
            var rate = await _currencyService.ConvertAsync(1, code, "AZN");

            var existingRate = await _context.CurrencyRates
                .FirstOrDefaultAsync(c => c.CurrencyCode == code && c.Date == DateTime.UtcNow.Date);

            if (existingRate == null)
            {
                var entity = new CurrencyRate
                {
                    CurrencyCode = code,
                    RateAgainstAzn = rate,
                    Date = DateTime.UtcNow.Date
                };

                _context.CurrencyRates.Add(entity);
            }
            else
            {
                existingRate.RateAgainstAzn = rate;
            }
        }

        await _context.SaveChangesAsync();

        await UpdateProductCurrenciesAsync();
    }

    private async Task UpdateProductCurrenciesAsync()
    {
        var todayRates = await _context.CurrencyRates
            .Where(c => c.Date == DateTime.UtcNow.Date)
            .ToListAsync();

        var products = await _context.Products.ToListAsync();

        foreach (var product in products)
        {
            decimal effectivePrice = product.DiscountedPercent > 0
                ? product.PriceAzn * (1 - product.DiscountedPercent / 100m)
                : product.PriceAzn;

            foreach (var rate in todayRates)
            {
                var convertedPrice = Math.Round(effectivePrice / rate.RateAgainstAzn, 2);

                var existing = await _context.ProductCurrencies
                    .FirstOrDefaultAsync(pc => pc.ProductId == product.Id && pc.CurrencyRateId == rate.Id);

                if (existing == null)
                {
                    _context.ProductCurrencies.Add(new ProductCurrency
                    {
                        ProductId = product.Id,
                        CurrencyRateId = rate.Id,
                        ConvertedPrice = convertedPrice
                    });
                }
                else
                {
                    existing.ConvertedPrice = convertedPrice;
                }
            }
        }

        await _context.SaveChangesAsync();
    }
}
