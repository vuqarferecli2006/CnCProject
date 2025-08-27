using CnC.Application.Abstracts.Repositories.IRepositories;
using CnC.Domain.Entities;

namespace CnC.Application.Abstracts.Repositories.IProductRepositories;

public interface IProductReadRepository:IReadRepository<Product>
{
    Task<CurrencyRate?> GetCurrencyRateByCodeAsync(string currencyCode);

    Task<Product?> GetByName(string productName);
}
