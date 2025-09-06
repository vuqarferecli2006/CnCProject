using CnC.Application.Abstracts.Repositories.IRepositories;
using CnC.Domain.Entities;

namespace CnC.Application.Abstracts.Repositories.ICurrencyRateRepository;

public interface ICurrencyRateReadRepository:IReadRepository<CurrencyRate>
{
    Task<CurrencyRate?> GetCurrencyRateByCodeAsync(string currencyCode, CancellationToken cancellationToken);
}
