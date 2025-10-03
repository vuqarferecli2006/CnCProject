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

    public async Task<List<PaymentMethod>> GetMethodWithUserAsync(string userId, CancellationToken ct)
    {
        return await _context.PaymentMethods
            .Where(pm => pm.UserId == userId)
            .OrderBy(pm => pm.CreatedAt) 
            .ToListAsync(ct);
    }
}
