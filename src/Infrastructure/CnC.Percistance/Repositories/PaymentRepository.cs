using CnC.Application.Abstracts.Repositories.IPaymentRepositories;
using CnC.Domain.Entities;
using CnC.Percistance.Contexts;

namespace CnC.Percistance.Repositories;

public class PaymentRepository : Repository<Payment>, IPaymentReadRepository, IPaymentWriteRepository
{
    public PaymentRepository(AppDbContext context) : base(context)
    {
    }
}
