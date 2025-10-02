using CnC.Application.Abstracts.Repositories.IPaymentMethodRepositories;
using CnC.Domain.Entities;
using CnC.Percistance.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CnC.Percistance.Repositories;

public class PaymentMethodRepository : Repository<PaymentMethod>, IPaymentMethodReadRepository, IPaymentMethodWriteRepository
{
    private readonly AppDbContext _context;
    public PaymentMethodRepository(AppDbContext context) : base(context)
    {
       _context = context;
    }

    public Task<PaymentMethod?> GetMethodWithUserAsync(Guid paymentMethodId)
    {
        throw new NotImplementedException();
    }
}
