using CnC.Application.Abstracts.Repositories.IRepositories;
using CnC.Domain.Entities;

namespace CnC.Application.Abstracts.Repositories.IPaymentMethodRepositories;

public interface IPaymentMethodReadRepository:IReadRepository<PaymentMethod>
{
    Task<List<PaymentMethod>> GetMethodWithUserAsync(string userId,CancellationToken ct);
}
