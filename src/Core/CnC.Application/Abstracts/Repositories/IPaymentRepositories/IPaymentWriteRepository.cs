using CnC.Application.Abstracts.Repositories.IRepositories;
using CnC.Domain.Entities;

namespace CnC.Application.Abstracts.Repositories.IPaymentRepositories;

public interface IPaymentWriteRepository:IWriteRepository<Payment>
{
    Task<Payment?> GetSingleAsync(Guid orderId,CancellationToken ct);
}
