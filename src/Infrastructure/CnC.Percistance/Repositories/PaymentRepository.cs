using CnC.Application.Abstracts.Repositories.IPaymentRepositories;
using CnC.Domain.Entities;
using CnC.Percistance.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CnC.Percistance.Repositories;

public class PaymentRepository : Repository<Payment>, IPaymentReadRepository, IPaymentWriteRepository
{
    private readonly AppDbContext _context;
    public PaymentRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Payment?> GetSingleAsync(Guid orderId, CancellationToken ct)
    {
        return await _context.Payments
            .FirstOrDefaultAsync(p => p.OrderId == orderId, ct);
    }
}
