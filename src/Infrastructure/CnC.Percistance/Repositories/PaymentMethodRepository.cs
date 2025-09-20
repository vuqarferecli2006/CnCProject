using CnC.Application.Abstracts.Repositories.IPaymentMethodRepositories;
using CnC.Domain.Entities;
using CnC.Percistance.Contexts;

namespace CnC.Percistance.Repositories;

public class PaymentMethodRepository : Repository<PaymentMethod>, IPaymentMethodReadRepository, IPaymentMethodWriteRepository
{
    public PaymentMethodRepository(AppDbContext context) : base(context)
    {
    }
}
