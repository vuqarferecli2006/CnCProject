using CnC.Application.Abstracts.Repositories.IRepositories;
using CnC.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CnC.Application.Abstracts.Repositories.IProductRepositories;

public interface IProductReadRepository:IReadRepository<Product>
{
    Task<CurrencyRate?> GetCurrencyRateByCodeAsync(string currencyCode);

    Task<Product?> GetByIdWithCurrenciesAsync(Guid id, CancellationToken ct);

    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken);


}
